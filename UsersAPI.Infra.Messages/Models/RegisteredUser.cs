using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Infra.Messages.Models
{
    public class RegisteredUser
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Role { get; set; }
    }
}
