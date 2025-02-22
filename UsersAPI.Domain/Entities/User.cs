using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Enums;

namespace UsersAPI.Domain.Entities
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Status? Status { get; set; }
        public Guid? RoleId { get; set; }

        #region Navegabilidades

        public Role? Role { get; set; }

        #endregion
    }
}
