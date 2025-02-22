using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.DTOs.Responses
{
    public class LoginUserResponseDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string? Token { get; set; }

        public DateTime? AccessedAt { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
