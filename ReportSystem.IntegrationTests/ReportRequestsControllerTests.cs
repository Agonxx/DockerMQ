using FluentAssertions;
using ReportSystem.API.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace ReportSystem.IntegrationTests
{
    // A classe de teste implementa IClassFixture, que diz ao xUnit para criar uma
    // única instância da nossa fábrica para todos os testes nesta classe.
    public class ReportRequestsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ReportRequestsControllerTests(CustomWebApplicationFactory factory)
        {
            // A fábrica nos dá um cliente HTTP pré-configurado para fazer chamadas à nossa API em memória.
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateReport_WithValidData_ShouldReturnCreated()
        {
            // Arrange (Organizar)
            // Criamos o corpo da nossa requisição.
            var createReportDto = new CreateReportRequestDto(
                ReportType: "MonthlySales",
                Parameters: "{ \"month\": 10, \"year\": 2024 }");

            // Act (Agir)
            // Enviamos uma requisição POST para nosso endpoint.
            var response = await _client.PostAsJsonAsync("/api/reportrequests", createReportDto);

            // Assert (Verificar)
            // Verificamos se a resposta foi a esperada.
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Verificamos se o header 'Location' foi retornado, como manda a boa prática do Created.
            response.Headers.Location.Should().NotBeNull();
        }
    }
}