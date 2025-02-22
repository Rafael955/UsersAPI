using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Infra.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //nome da tabela do banco de dados
            builder.ToTable("USER");

            //mapeamento da chave primária
            builder.HasKey(u => u.Id);

            //mapeamento dos demais campos
            builder.Property(u => u.Id)
                .HasColumnName("ID"); //nome do campo

            builder.Property(u => u.Name)
               .HasColumnName("NAME") //nome do campo
               .HasMaxLength(100) //tamanho máximo de caracteres
               .IsRequired(); //not null

            builder.Property(u => u.Email)
              .HasColumnName("EMAIL") //nome do campo
              .HasMaxLength(50) //tamanho máximo de caracteres
              .IsRequired(); //not null

            //criando um índice para definir o campo email como único
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password)
            .HasColumnName("PASSWORD") //nome do campo
            .HasMaxLength(100) //tamanho máximo de caracteres
            .IsRequired(); //not null

            builder.Property(u => u.Status)
           .HasColumnName("STATUS") //nome do campo
           .IsRequired(); //not null

            builder.Property(u => u.RoleId)
           .HasColumnName("ROLE_ID") //nome do campo
           .IsRequired(); //not null

            builder.HasOne(u => u.Role) //Usuário TEM 1 perfil
                .WithMany(r => r.Users) //Perfil TEM Muitos Usuários
                .HasForeignKey(u => u.RoleId); //Chave estrangeira
        }
    }
}
