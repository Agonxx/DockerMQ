using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportSystem.API.DTOs;
using ReportSystem.Application.ReportRequests.Commands.CreateReportRequest;

namespace ReportSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Injetamos a interface do MediatR. O controller não conhece o handler,
        // ele apenas sabe como "enviar" um comando.
        public ReportRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestDto requestDto)
        {
            // Mapeamos os dados do DTO para o nosso Command interno.
            var command = new CreateReportRequestCommand(
                "user-123", // Provisório: Em um sistema real, viria do token JWT.
                requestDto.ReportType,
                requestDto.Parameters);

            // Enviamos o comando para o MediatR, que encontrará e executará o handler correspondente.
            var reportId = await _mediator.Send(command);

            // Retornamos uma resposta 201 Created, que é a melhor prática para um POST bem-sucedido.
            // Incluímos o ID do novo recurso criado na resposta.
            return Created(nameof(CreateReport), new { id = reportId });
        }
    }
}