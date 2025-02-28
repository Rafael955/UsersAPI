using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsersAPI.Application.Security
{
    /// <summary>
    /// Classe para fazermos a geração dos TOKENS JWT
    /// </summary>
    public class JwtBearerSecurity
    {
        /// <summary>
        /// Método para retornar a data de expiração dos TOKENS
        /// </summary>
        public static DateTime? GetExpiration()
        {
            return DateTime.UtcNow.AddMinutes(30);
        }

        /// <summary>
        /// Método para retornar o token jwt de um determinado usuário
        /// </summary>
        public static string? GetAccessToken(Guid? usuarioId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //gravar a chave secreta que será utilizada para assinar os tokens
            var key = Encoding.ASCII.GetBytes("487C7A17-0D2E-4D3E-8F43-D29F865D4E75");

            //construindo o token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //gravando no corpo do TOKEN o ID do usuário autenticado
                Subject = new ClaimsIdentity(new Claim[]
                    { new Claim(ClaimTypes.Name, usuarioId.ToString()) }),

                //gravando a data e hora de expiração do token
                Expires = GetExpiration(),

                //gravando a assinatura do token feita com a chave secreta
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            //criando e retornando o token JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}



