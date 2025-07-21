using Microsoft.EntityFrameworkCore;
using ReportSystem.Domain.Aggregates;
using System.Reflection;

namespace ReportSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        // O construtor recebe as opções de configuração (como a connection string)
        // que serão injetadas pela nossa API.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Cada DbSet<T> corresponde a uma tabela no nosso banco de dados.
        public DbSet<ReportRequest> ReportRequests { get; set; }

        // Este método é onde configuramos o mapeamento das nossas entidades para as tabelas.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Isso instrui o EF Core a procurar por todas as configurações de entidade
            // que temos neste projeto (assembly) e aplicá-las.
            // É uma forma limpa de manter as configurações organizadas.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}