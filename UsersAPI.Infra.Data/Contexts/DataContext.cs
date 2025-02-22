using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Infra.Data.Mappings;

namespace UsersAPI.Infra.Data.Contexts
{
    public class DataContext : DbContext
    {
        //método para configurar a string de conexão do banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //adicionando a connectionString
            optionsBuilder.UseSqlServer("Data Source=\"localhost, 1434\";Initial Catalog=master;User ID=sa;Password=Coti@2025;Encrypt=False");
        }

        //método para adcionar cada classe de mapeamento do projeto
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //incluindo cada classe 'Map' no projeto
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
