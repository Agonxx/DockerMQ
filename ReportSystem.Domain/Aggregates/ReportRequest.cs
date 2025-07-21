using ReportSystem.Domain.Enums;
using System.Diagnostics;

namespace ReportSystem.Domain.Aggregates
{
    // A anotação [DebuggerDisplay] é fantástica! Ela muda como o objeto aparece
    // na janela de depuração do Visual Studio, tornando tudo mais fácil de entender.
    [DebuggerDisplay("{Status} - {ReportType} (Id: {Id})")]
    public class ReportRequest
    {
        // Propriedades com "private set" são o coração do encapsulamento.
        // Elas podem ser lidas de qualquer lugar, mas só podem ser definidas
        // DENTRO desta classe (no construtor ou em outros métodos).
        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public string ReportType { get; private set; }
        public string Parameters { get; private set; } // Guardaremos os parâmetros como um JSON.
        public ReportStatus Status { get; private set; }
        public DateTime RequestedAtUtc { get; private set; }
        public DateTime? ProcessedAtUtc { get; private set; } // Nullable, pois pode não ter sido processado ainda.
        public string? ReportUrl { get; private set; } // O link para download do relatório.
        public string? ErrorMessage { get; private set; }

        // Deixar o construtor privado força a criação do objeto através de
        // um método de fábrica (o "Create" abaixo), nos dando mais controle.
        private ReportRequest() { }

        // Este é um método de fábrica estático. É a forma "oficial" e segura
        // de criar uma nova solicitação de relatório válida.
        public static ReportRequest Create(string userId, string reportType, string parameters)
        {
            // Poderíamos adicionar validações aqui. Ex: if(string.IsNullOrEmpty(userId)) throw...
            return new ReportRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ReportType = reportType,
                Parameters = parameters,
                Status = ReportStatus.Pending, // O status inicial é sempre Pending.
                RequestedAtUtc = DateTime.UtcNow // Sempre usar UTC para datas no back-end.
            };
        }

        // === MÉTODOS QUE EXPÕEM COMPORTAMENTO ===
        // Em vez de um "set" público para o Status, temos métodos que descrevem Ações.
        // Isso é um Modelo de Domínio Rico!

        public void StartProcessing()
        {
            if (Status == ReportStatus.Pending)
            {
                Status = ReportStatus.Processing;
                ProcessedAtUtc = DateTime.UtcNow;
            }
        }

        public void CompleteSuccessfully(string reportUrl)
        {
            if (Status == ReportStatus.Processing)
            {
                Status = ReportStatus.Completed;
                ReportUrl = reportUrl;
                ProcessedAtUtc = DateTime.UtcNow; // Atualiza para o tempo de finalização.
            }
        }

        public void Fail(string errorMessage)
        {
            if (Status == ReportStatus.Processing)
            {
                Status = ReportStatus.Failed;
                ErrorMessage = errorMessage;
                ProcessedAtUtc = DateTime.UtcNow;
            }
        }
    }
}