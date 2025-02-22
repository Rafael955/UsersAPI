using System;
using System.Security.Cryptography;
using System.Text;

namespace UsersAPI.Infra.Data.Helpers
{
    /// <summary>
    /// Classe para criptografia de dados no padrão SHA256
    /// </summary>
    public class SHA256Encrypt
    {
        //Método estático para realizar a criptografia
        public static string Create(string value)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - retorna o hash em bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Converte os bytes em uma string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
