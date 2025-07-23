using MediatR; // Adicione este using
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReportSystem.Application.Abstractions.Interfaces; // Adicione este using
using ReportSystem.Application.ReportRequests.Commands.CreateReportRequest; // Adicione este using
using ReportSystem.Infrastructure.Persistence;
using ReportSystem.Infrastructure.Persistence.Repositories; // Adicione este using

namespace ReportSystem.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // 1. Definimos o ambiente para "IntegrationTest".
            // Isso fará com que o 'if' no nosso Program.cs pule o registro do SQL Server.
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                // Remover o DbContext original do SQL Server
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                // Adicionar o DbContext em memória
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"InMemoryDbForTesting-{Guid.NewGuid()}");
                });

                // ================================================================
                // GARANTIR QUE OS SERVIÇOS DA APLICAÇÃO ESTÃO REGISTRADOS
                // ================================================================
                // Mesmo que o Program.cs já faça isso, ser explícito no ambiente de
                // teste pode resolver problemas de descoberta de assemblies.

                // 1. Registrar o MediatR novamente para o assembly da Aplicação
                services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(typeof(CreateReportRequestCommand).Assembly));

                // 2. Registrar nosso Repositório novamente
                services.AddScoped<IReportRequestRepository, ReportRequestRepository>();
            });
        }
    }
}