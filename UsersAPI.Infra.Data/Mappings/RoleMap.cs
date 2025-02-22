using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Infra.Data.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            //Nome da tabela do banco de dados
            builder.ToTable("TB_ROLE");

            //chave primária
            builder.HasKey(r => r.Id);

            //mapeamento dos campos da tabela
            builder.Property(r => r.Id)
                .HasColumnName("ID");

            //mapeamento dos campos da tabela
            builder.Property(r => r.Name)
                .HasColumnName("NAME") //nome do campo
                .HasMaxLength(25) //tamanho máximo de caracteres
                .IsRequired(); //not null

            //defininfo o campo 'Name' como único na tabela
            builder.HasIndex(r => r.Name).IsUnique();
        }
    }
}
