using MediatR;
using ReportSystem.Application.Abstractions.Interfaces;
using ReportSystem.Domain.Aggregates;

namespace ReportSystem.Application.ReportRequests.Commands.CreateReportRequest
{
    // Esta classe é nosso Handler. Ela implementa a interface IRequestHandler,
    // dizendo ao MediatR que ela sabe como lidar com um CreateReportRequestCommand.
    public class CreateReportRequestCommandHandler : IRequestHandler<CreateReportRequestCommand, Guid>
    {
        private readonly IReportRequestRepository _reportRequestRepository;

        // Injetamos a abstração do nosso repositório. O handler não sabe se os dados
        // vão para um SQL Server ou um arquivo de texto, ele apenas conhece o contrato.
        public CreateReportRequestCommandHandler(IReportRequestRepository reportRequestRepository)
        {
            _reportRequestRepository = reportRequestRepository;
        }

        // Este é o método que o MediatR vai chamar.
        public async Task<Guid> Handle(CreateReportRequestCommand request, CancellationToken cancellationToken)
        {
            // 1. Usamos nosso método de fábrica do Domínio para criar a entidade.
            // A lógica de criação (definir status como Pending, gerar ID) está protegida.
            var reportRequest = ReportRequest.Create(
                request.UserId,
                request.ReportType,
                request.Parameters);

            // 2. Usamos o repositório para persistir a nova entidade.
            await _reportRequestRepository.AddAsync(reportRequest);

            // 3. Retornamos o ID da nova entidade, cumprindo o contrato da interface.
            return reportRequest.Id;
        }
    }
}