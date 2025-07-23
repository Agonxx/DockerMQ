using System.ComponentModel.DataAnnotations;

namespace ReportSystem.API.DTOs
{
    // Este é o "corpo" do nosso pedido POST.
    // Usamos anotações de validação para garantir que os dados recebidos são válidos.
    public record CreateReportRequestDto(
        [Required]
        [StringLength(100)]
        string ReportType,

        [Required]
        string Parameters);
}