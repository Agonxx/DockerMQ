using ReportSystem.Application.Abstractions.Interfaces;
using ReportSystem.Infrastructure.Persistence;
using ReportSystem.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner.

// 1. Configurar o DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Registrar nosso Reposit�rio (Mapear a interface para a implementa��o)
// Quando um construtor pedir por um IReportRequestRepository, o DI fornecer� um ReportRequestRepository.
builder.Services.AddScoped<IReportRequestRepository, ReportRequestRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();