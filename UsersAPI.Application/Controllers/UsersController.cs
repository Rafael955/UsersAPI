using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Application.Exceptions;
using UsersAPI.Application.Security;
using UsersAPI.Domain.DTOs.Requests;
using UsersAPI.Domain.DTOs.Responses;
using UsersAPI.Domain.Entities;
using UsersAPI.Domain.Enums;
using UsersAPI.Infra.Data.Helpers;
using UsersAPI.Infra.Data.Repositories;
using UsersAPI.Infra.Messages.Models;
using UsersAPI.Infra.Messages.Producers;

namespace UsersAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly RoleRepository _roleRepository = new RoleRepository(); 

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateUserResponseDto), 201)]
        public IActionResult Create([FromBody] CreateUserRequestDto request)
        {
            try
            {
                //verificar se o email informado já existe na base de dados
                if (_userRepository.HasEmail(request.Email))
                    throw new EmailAlreadyRegisteredException();

                //preenchendo os dados do usuário
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    Password = SHA256Encrypt.Create(request.Password),
                    Status = Status.Active,
                    RoleId = _roleRepository.GetByName("OPERADOR")?.Id
                };

                //gravando o usuário no banco de dados
                _userRepository.Execute(user, Infra.Data.Enums.OperationType.ADD);

                //enviando o usuário cadastrado para a fila do RabbitMQ
                var messageProducer = new MessageProducer();
                messageProducer.SendMessage(new RegisteredUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Role = "OPERADOR"
                });

                //retornando sucesso HTTP 201 (CREATED)
                return StatusCode(StatusCodes.Status201Created, new CreateUserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Role = "OPERADOR"
                });
            }
            catch(EmailAlreadyRegisteredException ex)
            {
                //HTTP 400 (Erro do cliente): Bad Request
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                //HTTP 500 (Erro do Servidor): Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponseDto), 200)]
        public IActionResult Login([FromBody] LoginUserRequestDto request)
        {
            try
            {
                //consultar o usuário no banco de dados através do email e da senha
                var user = _userRepository.GetByEmailAndPassword(request.Email, SHA256Encrypt.Create(request.Password));

                if (user == null)
                    throw new UnauthorizedAccessException("Acesso Negado. Usuário não encontrado.");
                    
                //retornar os dados do usuário autenticado
                var response = new LoginUserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role.Name,
                    AccessedAt = DateTime.Now,
                    Token = JwtBearerSecurity.GetAccessToken(user.Id),
                    Expiration = JwtBearerSecurity.GetExpiration()
                };

                //retornar sucesso
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch(UnauthorizedAccessException ex)
            {
                //HTTP 401 (Unauthorized): Acesso não autorizado
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                //HTTP 500 (Erro do Servidor): Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
