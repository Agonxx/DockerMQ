namespace ReportSystem.Domain.Enums
{
    public enum ReportStatus
    {
        Pending,      // O pedido foi criado e está na fila, aguardando processamento.
        Processing,   // O Worker pegou o pedido e está ativamente gerando o relatório.
        Completed,    // O relatório foi gerado com sucesso e está disponível para download.
        Failed        // Ocorreu um erro durante a geração do relatório.
    }
}
