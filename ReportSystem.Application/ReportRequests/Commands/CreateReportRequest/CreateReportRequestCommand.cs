using MediatR;

namespace ReportSystem.Application.ReportRequests.Commands.CreateReportRequest
{
    public record CreateReportRequestCommand(
        string UserId,
        string ReportType,
        string Parameters) : IRequest<Guid>;
}
