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
    /// Repositorio de dados para a entidade 'Perfil (Role)'
    /// </summary>
    public class RoleRepository
    {
        public void Execute(RoleRepository role, OperationType operation)
        {
            using (var context = new DataContext())
            {
                switch (operation)
                {
                    case OperationType.ADD:
                        context.Add(role);
                        break;
                    case OperationType.UPDATE:
                        context.Update(role);
                        break;
                    case OperationType.DELETE:
                        context.Remove(role);
                        break;
                }

                context.SaveChanges();
            }
        }


        //Método para consultar todos os perfis do banco de dados
        public List<Role> GetAll()
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<Role>().OrderBy(r => r.Name).ToList();

                //LINQ (Language Integrated Query)
                var query = from r in dataContext.Set<Role>()
                            orderby r.Name ascending
                            select r;

                return query.ToList();
            }
        }

        //Método para consultar 1 perfil através do nome
        public Role? GetByName(string name)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<Role>().FirstOrDefault(r => r.Name.Equals(name));

                //LINQ (Language Integrated Query)
                var query = from r in dataContext.Set<Role>()
                            where r.Name.Equals(name)
                            select r;

                return query.FirstOrDefault();
            }
        }
    }
}
