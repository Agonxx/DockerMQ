// ========================================================================================
// SE��O DE USINGS
// ========================================================================================
// Aqui importamos os namespaces que cont�m as classes e interfaces que usaremos abaixo.
// � uma boa pr�tica manter apenas os 'usings' necess�rios.
using Microsoft.EntityFrameworkCore;
using ReportSystem.Application.Abstractions.Interfaces; // Para nossa interface IReportRequestRepository
using ReportSystem.Application.ReportRequests.Commands.CreateReportRequest; // Para o nosso Command, usado pelo MediatR
using ReportSystem.Infrastructure.Persistence; // Para nosso AppDbContext
using ReportSystem.Infrastructure.Persistence.Repositories; // Para nossa implementa��o ReportRequestRepository

// ========================================================================================
// SE��O DE CONFIGURA��O DE SERVI�OS (O "ANTES" DA APLICA��O)
// ========================================================================================
// Tudo o que acontece antes de "var app = builder.Build();" � a configura��o do
// "motor" da nossa aplica��o. � aqui que definimos como as pe�as se encaixam,
// quais servi�os est�o dispon�veis e como eles devem ser constru�dos.
// Isso � o Cont�iner de Inje��o de Depend�ncia (DI Container) em a��o.

var builder = WebApplication.CreateBuilder(args);

// --- Adicionar servi�os ao cont�iner. ---

// 1. CONFIGURAR O ENTITY FRAMEWORK CORE (AppDbContext)
// Esta se��o prepara a conex�o com o banco de dados.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    // Aqui dizemos ao EF Core para usar o provedor do SQL Server e fornecemos a
    // connection string que ele ler� do nosso arquivo appsettings.json.
    options.UseSqlServer(connectionString));

// 2. REGISTRAR NOSSOS PR�PRIOS SERVI�OS (REPOSIT�RIO)
// A ordem aqui geralmente n�o importa, desde que esteja antes de `builder.Build()`.
// � uma boa pr�tica registrar os servi�os relacionados � infraestrutura primeiro.
builder.Services.AddScoped<IReportRequestRepository, ReportRequestRepository>();
// Explica��o:
// 'AddScoped' define o "tempo de vida" do nosso servi�o. Significa que uma nova
// inst�ncia de 'ReportRequestRepository' ser� criada para cada requisi��o HTTP.
// Se m�ltiplos servi�os pedirem um IReportRequestRepository dentro da mesma requisi��o,
// eles receber�o a MESMA inst�ncia, o que � perfeito para o nosso DbContext.
// A linha diz: "Quando algu�m pedir (injetar) um 'IReportRequestRepository',
// entregue uma inst�ncia de 'ReportRequestRepository'". Isso � o mapeamento
// da abstra��o (interface) para a implementa��o (classe concreta).

// 3. REGISTRAR O MEDIATR
// Este servi�o orquestrar� nossos Commands e Handlers.
builder.Services.AddMediatR(cfg =>
    // Esta linha � a m�gica do MediatR. Ela diz: "Encontre a classe 'CreateReportRequestCommand',
    // olhe para todo o projeto (Assembly) onde ela est� declarada (nosso ReportSystem.Application),
    // e registre automaticamente todas as interfaces (IRequest) e todos os handlers (IRequestHandler)
    // que voc� encontrar l�."
    cfg.RegisterServicesFromAssembly(typeof(CreateReportRequestCommand).Assembly));

// 4. CONFIGURAR SERVI�OS PADR�O DO ASP.NET CORE MVC/API
builder.Services.AddControllers();
// Habilita o uso de Controllers para lidar com as requisi��es HTTP. Sem isso,
// nossos controllers n�o seriam descobertos ou executados.

builder.Services.AddEndpointsApiExplorer();
// Necess�rio para que o Swagger (abaixo) consiga descobrir e entender os endpoints
// da nossa API para gerar a documenta��o.

builder.Services.AddSwaggerGen();
// Habilita o servi�o de gera��o de documenta��o Swagger/OpenAPI. � ele que cria
// a p�gina interativa do Swagger que usamos para testar a API.

// ========================================================================================
// SE��O DE CONSTRU��O E CONFIGURA��O DO PIPELINE (O "DEPOIS")
// ========================================================================================
// Aqui, a "m�gica" acontece. O builder pega todos os servi�os que registramos
// e constr�i a aplica��o web de fato.
var app = builder.Build();

// Tudo o que acontece DEPOIS de "builder.Build()" define o PIPELINE de requisi��es HTTP.
// Pense nisso como uma "linha de montagem". Cada requisi��o que chega passa por
// cada um destes 'app.Use...()' na ordem em que est�o escritos.
// A ORDEM AQUI � EXTREMAMENTE IMPORTANTE!

// Configure the HTTP request pipeline.
// Esta verifica��o garante que as ferramentas de desenvolvedor (como o Swagger)
// s� sejam habilitadas quando a aplica��o est� rodando em ambiente de desenvolvimento.
// Isso evita expor a documenta��o da API em um ambiente de produ��o.
if (app.Environment.IsDevelopment())
{
    // Habilita o middleware que gera o arquivo JSON da especifica��o OpenAPI.
    app.UseSwagger();
    // Habilita o middleware que serve a interface de usu�rio (UI) do Swagger.
    // � importante que UseSwaggerUI() venha DEPOIS de UseSwagger().
    app.UseSwaggerUI();
}

// Comentado para nosso ambiente Docker, pois n�o estamos gerenciando certificados.
// Se estivesse ativo, redirecionaria todas as requisi��es HTTP para HTTPS.
// app.UseHttpsRedirection();

// Habilita o middleware de autoriza��o. Ele verifica se um usu�rio tem permiss�o
// para acessar um determinado endpoint (usando atributos como [Authorize]).
// Deve vir antes de MapControllers().
app.UseAuthorization();

// Mapeia as requisi��es HTTP para as a��es corretas nos nossos controllers com base
// nas rotas que definimos (ex: [Route("api/[controller]")]).
// Esta deve ser uma das �ltimas coisas no pipeline.
app.MapControllers();

// Inicia a aplica��o e a faz come�ar a escutar por requisi��es HTTP.
// Este � um comando de bloqueio; o c�digo para de executar aqui at� que a aplica��o seja encerrada.
app.Run();