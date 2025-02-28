using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.DTOs.Requests;
using UsersAPI.Domain.DTOs.Responses;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Tests.IntegrationTests
{
    /// <summary>
    /// Classe para testes dos serviços de usuários
    /// </summary>
    public class UsersTest
    {
        [Fact]
        public void CreateUser_Successfully()
        {
            //criando os dados do teste
            var request = new Faker<CreateUserRequestDto>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, "Teste123$")
                .RuleFor(u => u.PasswordConfirm, "Teste123$")
                .Generate();

            //serializar os dados da requisição em JSON
            var jsonRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            //criando a requisição / solicitação para a API
            var client = new WebApplicationFactory<Program>().CreateClient();

            //executando a chamada para o endpoint de cadastro de usuário
            var response = client.PostAsync("/api/users/create", jsonRequest).Result;

            //verificando se a API retornou código 201 (HTTP CREATED)
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            //capturando a resposta obtida pela API
            var content = response.Content.ReadAsStringAsync()?.Result;//json

            //deserializando o JSON de resposta retornado pela API
            var usuario = JsonConvert.DeserializeObject<CreateUserResponseDto>(content);

            //verificando o conteudo dos dados do usuário
            usuario?.Id.Should().NotBeNull(); //Verificando se o usuário possui um Id
            usuario?.Name.Should().Be(request.Name); //comparando o nome do usuário
            usuario?.Email.Should().Be(request.Email); //comparando o email do usuário
            usuario?.Role.Should().Be("OPERADOR");  //comparando o perfil do usuário
            usuario?.CreatedAt.Should().NotBeNull(); //Verificando se tem data e hora de cadastro
        }

        [Fact]
        public void CreateUser_EmailAlreadyRegistered()
        {
            //criando os dados do teste
            var request = new Faker<CreateUserRequestDto>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, "Teste123$")
                .RuleFor(u => u.PasswordConfirm, "Teste123$")
                .Generate();

            //serializar os dados da requisição em JSON
            var jsonRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            //criando a requisição / solicitação para a API
            var client = new WebApplicationFactory<Program>().CreateClient();

            //executando a chamada para o endpoint de cadastro de usuário
            var response = client.PostAsync("/api/users/create", jsonRequest).Result;

            //verificando se a API retornou código 201 (HTTP CREATED)
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            //executando o cadastro do mesmo usuário novamente
            var responseUsuarioJaCadastrado = client.PostAsync("/api/users/create", jsonRequest)?.Result;

            //verificando se a API retornou código 400 (HTTP BAD REQUEST)
            responseUsuarioJaCadastrado?.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //capturando a resposta da API
            var content = responseUsuarioJaCadastrado?.Content.ReadAsStringAsync()?.Result;//json

            //verificando se a mensagem de erro foi retornada pela API
            content.Should().Contain($"O email informado já está cadastrado. Tente outro.");
        }

        [Fact]
        public void LoginUser_Successfully()
        {
            //criando os dados para cadastro do usuário
            var requestCadastro = new Faker<CreateUserRequestDto>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, "Teste123$")
                .RuleFor(u => u.PasswordConfirm, "Teste123$")
                .Generate();

            //serializando os dados da requisição em JSON
            var jsonRequestCadastro = new StringContent(JsonConvert.SerializeObject(requestCadastro),
                                    Encoding.UTF8, "application/json");

            //criando a requisição / solicitação para a API
            var client = new WebApplicationFactory<Program>().CreateClient();

            //executando a chamada para o endpoint de cadastro de usuário
            var responseCadastro = client.PostAsync("/api/users/create", jsonRequestCadastro)?.Result;

            //criando os dados para autenticação do usuário
            var requestLogin = new LoginUserRequestDto
            {
                Email = requestCadastro.Email, //email do usuário cadastrado
                Password = requestCadastro.Password, //senha do usuário cadastrado
            };

            //serializando os dados da requisição em JSON
            var jsonRequestLogin = new StringContent(JsonConvert.SerializeObject(requestLogin),
                                        Encoding.UTF8, "application/json");

            //executando a chamada para o endpoint de login de usuário
            var responseLogin = client.PostAsync("/api/users/login", jsonRequestLogin)?.Result;

            //verificando se a API retornou código 200 (HTTP OK)
            responseLogin?.StatusCode.Should().Be(HttpStatusCode.OK);

            //capturando a resposta obtida pela API
            var content = responseLogin?.Content.ReadAsStringAsync()?.Result;

            //deserializando o JSON de resposta retornado pela API
            var usuario = JsonConvert.DeserializeObject<LoginUserResponseDto>(content);

            //verificando o conteudo dos dados do usuário
            usuario?.Id.Should().NotBeNull(); //verificando se o usuário posssui um ID
            usuario?.Name.Should().Be(requestCadastro.Name); //comparando o nome do usuário
            usuario?.Email.Should().Be(requestCadastro.Email); //comparando o email do usuário
            usuario?.Role.Should().Be("OPERADOR"); //comparando o perfil do usuário
            usuario?.Token.Should().NotBeEmpty(); //verificando se um token foi gerado
            usuario?.Expiration.Should().NotBeNull(); //verificando se foi gerado uma data de expiração
        }


        [Fact]
        public void LoginUser_AccessDenied()
        {
            //criando os dados para autenticação do usuário
            var request = new Faker<LoginUserRequestDto>()
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, "@Usuario2025")
                .Generate();

            //serializando os dados da requisição em JSON
            var jsonRequest = new StringContent(JsonConvert.SerializeObject(request),
                                    Encoding.UTF8, "application/json");

            //criando a requisição / solicitação para a API
            var client = new WebApplicationFactory<Program>().CreateClient();

            //executando a chamada para o endpoint de autenticação de usuário
            var response = client.PostAsync("/api/users/login", jsonRequest)?.Result;

            //verificando se a API retornou código 401 (HTTP UNAUTHORIZED)
            response?.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            //capturando a resposta obtida pela API
            var content = response?.Content.ReadAsStringAsync()?.Result;

            //verificando o conteúdo retornado pela resposta
            content.Should().Contain("Acesso Negado. Usuário não encontrado.");
        }
    }
}
