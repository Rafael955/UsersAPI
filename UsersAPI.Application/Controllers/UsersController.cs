using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Application.Exceptions;
using UsersAPI.Domain.DTOs.Requests;
using UsersAPI.Domain.DTOs.Responses;
using UsersAPI.Domain.Entities;
using UsersAPI.Domain.Enums;
using UsersAPI.Infra.Data.Helpers;
using UsersAPI.Infra.Data.Repositories;

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
                    throw new DuplicatedEmailException($"O email informado já está cadastrado. Tente outro.");

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

                //retornando sucesso HTTP 201 (CREATED)
                return StatusCode(201, new CreateUserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Role = "OPERADOR"
                });
            }
            catch(DuplicatedEmailException ex)
            {
                //HTTP 400 (Erro do cliente): Bad Request
                return StatusCode(400, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                //HTTP 500 (Erro do Servidor): Internal Server Error
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponseDto), 200)]
        public IActionResult Login([FromBody] LoginUserRequestDto request)
        {
            return Ok();
        }
    }
}
