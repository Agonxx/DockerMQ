using ReportSystem.Domain.Aggregates;

namespace ReportSystem.Application.Abstractions.Interfaces
{
    // Esta interface é o nosso contrato.
    // Qualquer classe que queira atuar como um repositório para ReportRequest
    // DEVE implementar estes métodos.
    public interface IReportRequestRepository
    {
        /// <summary>
        /// Adiciona uma nova solicitação de relatório ao armazenamento de dados.
        /// </summary>
        /// <param name="reportRequest">A entidade de solicitação de relatório a ser adicionada.</param>
        Task AddAsync(ReportRequest reportRequest);

        /// <summary>
        /// Busca uma solicitação de relatório pelo seu identificador único.
        /// </summary>
        /// <param name="id">O ID da solicitação a ser buscada.</param>
        /// <returns>A entidade encontrada ou null se não existir.</returns>
        Task<ReportRequest?> GetByIdAsync(Guid id);

        /// <summary>
        /// Atualiza uma solicitação de relatório existente no armazenamento de dados.
        /// </summary>
        /// <param name="reportRequest">A entidade de solicitação de relatório a ser atualizada.</param>
        Task UpdateAsync(ReportRequest reportRequest);
    }
}