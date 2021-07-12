using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            // Define a chave primaria
            builder.HasKey(p => p.Id);

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Logradouro)
                .IsRequired()
                .HasColumnType("varchar(200)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Numero)
                .IsRequired()
                .HasColumnType("varchar(50)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Cep)
                .IsRequired()
                .HasColumnType("varchar(8)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Complemento)               
                .HasColumnType("varchar(250)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Bairro)
                .IsRequired()
                .HasColumnType("varchar(100)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Cidade)
                .IsRequired()
                .HasColumnType("varchar(100)");

            // Define o campo como requirido, define o tipo da coluna            
            builder.Property(c => c.Estado)
                .IsRequired()
                .HasColumnType("varchar(50)");
                       
            builder.ToTable("Enderecos");
        }
    }
}
