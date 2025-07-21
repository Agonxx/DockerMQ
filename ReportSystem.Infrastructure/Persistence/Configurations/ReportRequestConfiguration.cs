using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportSystem.Domain.Aggregates;
using ReportSystem.Domain.Enums;

namespace ReportSystem.Infrastructure.Persistence.Configurations
{
    public class ReportRequestConfiguration : IEntityTypeConfiguration<ReportRequest>
    {
        public void Configure(EntityTypeBuilder<ReportRequest> builder)
        {
            // Define a chave primária da tabela.
            builder.HasKey(r => r.Id);

            // Mapeia a propriedade ReportType para uma coluna VARCHAR(100) e a torna obrigatória.
            builder.Property(r => r.ReportType)
                .HasMaxLength(100)
                .IsRequired();

            // Mapeia a propriedade UserId para uma coluna VARCHAR(100) e a torna obrigatória.
            builder.Property(r => r.UserId)
                .HasMaxLength(100)
                .IsRequired();

            // Uma configuração muito importante: diz ao EF Core para salvar nossa enum
            // ReportStatus como uma string no banco (ex: "Pending", "Completed").
            // Isso torna os dados no banco muito mais legíveis do que se salvasse o número (0, 1, 2).
            builder.Property(r => r.Status)
                .HasConversion(
                    status => status.ToString(),
                    value => (ReportStatus)Enum.Parse(typeof(ReportStatus), value)
                );

            // Podemos configurar outros campos aqui se necessário.
            builder.Property(r => r.Parameters).IsRequired();
            builder.Property(r => r.ReportUrl).HasMaxLength(500);
            builder.Property(r => r.ErrorMessage).HasMaxLength(1000);
        }
    }
}