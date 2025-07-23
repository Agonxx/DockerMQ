// ========================================================================================
// SEÇÃO DE USINGS
// ========================================================================================
// Aqui importamos os namespaces que contêm as classes e interfaces que usaremos abaixo.
// É uma boa prática manter apenas os 'usings' necessários.
using Microsoft.EntityFrameworkCore;
using ReportSystem.Application.Abstractions.Interfaces; // Para nossa interface IReportRequestRepository
using ReportSystem.Application.ReportRequests.Commands.CreateReportRequest; // Para o nosso Command, usado pelo MediatR
using ReportSystem.Infrastructure.Persistence; // Para nosso AppDbContext
using ReportSystem.Infrastructure.Persistence.Repositories; // Para nossa implementação ReportRequestRepository

// ========================================================================================
// SEÇÃO DE CONFIGURAÇÃO DE SERVIÇOS (O "ANTES" DA APLICAÇÃO)
// ========================================================================================
// Tudo o que acontece antes de "var app = builder.Build();" é a configuração do
// "motor" da nossa aplicação. É aqui que definimos como as peças se encaixam,
// quais serviços estão disponíveis e como eles devem ser construídos.
// Isso é o Contêiner de Injeção de Dependência (DI Container) em ação.

var builder = WebApplication.CreateBuilder(args);

// --- Adicionar serviços ao contêiner. ---

// 1. CONFIGURAR O ENTITY FRAMEWORK CORE (AppDbContext)
// Esta seção prepara a conexão com o banco de dados.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    // Aqui dizemos ao EF Core para usar o provedor do SQL Server e fornecemos a
    // connection string que ele lerá do nosso arquivo appsettings.json.
    options.UseSqlServer(connectionString));

// 2. REGISTRAR NOSSOS PRÓPRIOS SERVIÇOS (REPOSITÓRIO)
// A ordem aqui geralmente não importa, desde que esteja antes de `builder.Build()`.
// É uma boa prática registrar os serviços relacionados à infraestrutura primeiro.
builder.Services.AddScoped<IReportRequestRepository, ReportRequestRepository>();
// Explicação:
// 'AddScoped' define o "tempo de vida" do nosso serviço. Significa que uma nova
// instância de 'ReportRequestRepository' será criada para cada requisição HTTP.
// Se múltiplos serviços pedirem um IReportRequestRepository dentro da mesma requisição,
// eles receberão a MESMA instância, o que é perfeito para o nosso DbContext.
// A linha diz: "Quando alguém pedir (injetar) um 'IReportRequestRepository',
// entregue uma instância de 'ReportRequestRepository'". Isso é o mapeamento
// da abstração (interface) para a implementação (classe concreta).

// 3. REGISTRAR O MEDIATR
// Este serviço orquestrará nossos Commands e Handlers.
builder.Services.AddMediatR(cfg =>
    // Esta linha é a mágica do MediatR. Ela diz: "Encontre a classe 'CreateReportRequestCommand',
    // olhe para todo o projeto (Assembly) onde ela está declarada (nosso ReportSystem.Application),
    // e registre automaticamente todas as interfaces (IRequest) e todos os handlers (IRequestHandler)
    // que você encontrar lá."
    cfg.RegisterServicesFromAssembly(typeof(CreateReportRequestCommand).Assembly));

// 4. CONFIGURAR SERVIÇOS PADRÃO DO ASP.NET CORE MVC/API
builder.Services.AddControllers();
// Habilita o uso de Controllers para lidar com as requisições HTTP. Sem isso,
// nossos controllers não seriam descobertos ou executados.

builder.Services.AddEndpointsApiExplorer();
// Necessário para que o Swagger (abaixo) consiga descobrir e entender os endpoints
// da nossa API para gerar a documentação.

builder.Services.AddSwaggerGen();
// Habilita o serviço de geração de documentação Swagger/OpenAPI. É ele que cria
// a página interativa do Swagger que usamos para testar a API.

// ========================================================================================
// SEÇÃO DE CONSTRUÇÃO E CONFIGURAÇÃO DO PIPELINE (O "DEPOIS")
// ========================================================================================
// Aqui, a "mágica" acontece. O builder pega todos os serviços que registramos
// e constrói a aplicação web de fato.
var app = builder.Build();

// Tudo o que acontece DEPOIS de "builder.Build()" define o PIPELINE de requisições HTTP.
// Pense nisso como uma "linha de montagem". Cada requisição que chega passa por
// cada um destes 'app.Use...()' na ordem em que estão escritos.
// A ORDEM AQUI É EXTREMAMENTE IMPORTANTE!

// Configure the HTTP request pipeline.
// Esta verificação garante que as ferramentas de desenvolvedor (como o Swagger)
// só sejam habilitadas quando a aplicação está rodando em ambiente de desenvolvimento.
// Isso evita expor a documentação da API em um ambiente de produção.
if (app.Environment.IsDevelopment())
{
    // Habilita o middleware que gera o arquivo JSON da especificação OpenAPI.
    app.UseSwagger();
    // Habilita o middleware que serve a interface de usuário (UI) do Swagger.
    // É importante que UseSwaggerUI() venha DEPOIS de UseSwagger().
    app.UseSwaggerUI();
}

// Comentado para nosso ambiente Docker, pois não estamos gerenciando certificados.
// Se estivesse ativo, redirecionaria todas as requisições HTTP para HTTPS.
// app.UseHttpsRedirection();

// Habilita o middleware de autorização. Ele verifica se um usuário tem permissão
// para acessar um determinado endpoint (usando atributos como [Authorize]).
// Deve vir antes de MapControllers().
app.UseAuthorization();

// Mapeia as requisições HTTP para as ações corretas nos nossos controllers com base
// nas rotas que definimos (ex: [Route("api/[controller]")]).
// Esta deve ser uma das últimas coisas no pipeline.
app.MapControllers();

// Inicia a aplicação e a faz começar a escutar por requisições HTTP.
// Este é um comando de bloqueio; o código para de executar aqui até que a aplicação seja encerrada.
app.Run();