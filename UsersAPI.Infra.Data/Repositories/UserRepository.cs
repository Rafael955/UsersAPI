using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;
using UsersAPI.Infra.Data.Contexts;
using UsersAPI.Infra.Data.Enums;

namespace UsersAPI.Infra.Data.Repositories
{
    /// <summary>
    /// Repositório de dados para a entidade 'Usuário (User)'
    /// </summary>
    public class UserRepository
    {
        public void Execute(User user, OperationType operation)
        {
            using (var dataContext = new DataContext())
            {
                switch (operation)
                {
                    case OperationType.ADD:
                        dataContext.Add(user);
                        break;
                    case OperationType.UPDATE:
                        dataContext.Update(user);
                        break;
                    case OperationType.DELETE:
                        dataContext.Remove(user);
                        break;
                    default:
                        break;
                }

                dataContext.SaveChanges();
            }
        }

        //Mètodo para consultar 1 usuário através do email e da senha
        public User? GetByEmailAndPassword(string email, string password)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                return dataContext.Set<User>()
                    .Include(x => x.Role)
                    .Where(u => u.Email.Equals(email) && u.Password.Equals(password))
                    .FirstOrDefault();

                //LINQ
                //var query = from u in dataContext.Set<User>()
                //            join r in dataContext.Set<Role>()
                //            on u.RoleId equals r.Id
                //            where u.Email.Equals(email) && u.Password.Equals(password)
                //            select u;

                //return query.FirstOrDefault();
            }
        }

        public bool HasEmail(string email)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<User>().Where(u => u.Email.Equals(email)).Any();

                //LINQ
                var query = from u in dataContext.Set<User>()
                            where u.Email.Equals(email)
                            select u;

                return query.Any();
            }
        }
    }
}
