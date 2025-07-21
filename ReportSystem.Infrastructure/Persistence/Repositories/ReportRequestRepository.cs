using ReportSystem.Application.Abstractions.Interfaces;
using ReportSystem.Domain.Aggregates;

namespace ReportSystem.Infrastructure.Persistence.Repositories
{
    // Esta classe implementa a interface que definimos na camada de Aplicação.
    // Ela é a ponte concreta entre nossa lógica de negócio e o banco de dados.
    public class ReportRequestRepository : IReportRequestRepository
    {
        private readonly AppDbContext _context;

        // Usamos Injeção de Dependência para receber uma instância do nosso AppDbContext.
        // Isso nos desacopla da criação do contexto e facilita os testes.
        public ReportRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReportRequest reportRequest)
        {
            // Adiciona a entidade ao "rastreador de mudanças" do EF Core.
            await _context.ReportRequests.AddAsync(reportRequest);

            // O comando SaveChangesAsync é o que efetivamente envia os comandos
            // (neste caso, um INSERT) para o banco de dados.
            await _context.SaveChangesAsync();
        }

        public async Task<ReportRequest?> GetByIdAsync(Guid id)
        {
            // Usa o método FindAsync, que é otimizado para buscar uma entidade
            // pela sua chave primária.
            return await _context.ReportRequests.FindAsync(id);
        }

        public async Task UpdateAsync(ReportRequest reportRequest)
        {
            // O EF Core é inteligente. Ele já sabe que a entidade 'reportRequest'
            // que recebemos pode ter sido modificada. Apenas precisamos dizer a ele
            // para salvar as mudanças.
            _context.ReportRequests.Update(reportRequest);
            await _context.SaveChangesAsync();
        }
    }
}