using Serilog;
using ApiDapperClean.Api.Middlewares;
using ApiDapperClean.Api.Filters;
using ApiDapperClean.Infrastructure.Migrations;
using Scalar.AspNetCore;
using Serilog.Events;
using FluentValidation;
using ApiDapperClean.Application.Validators;
using ApiDapperClean.Application.Services.v1;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Infrastructure.Data;
using ApiDapperClean.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton(Log.Logger);

// Adicionar serviços
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "ApiDapperClean";
        document.Info.Version = "v1";
        document.Info.Description = "API com arquitetura limpa usando Dapper e PostgreSQL";
        document.Info.Contact = new()
        {
            Name = "Seu Nome",
            Email = "seu.email@example.com"
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registrar validadores
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

// Registrar serviços
builder.Services.AddScoped<IProductService, ProductService>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// Registrar DbConnection
builder.Services.AddScoped<IDbConnection>(_ => new DbConnection(connectionString));

// Registrar repositórios
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

var app = builder.Build();

// Inicializar banco de dados
using (var scope = app.Services.CreateScope())
{
    try
    {
        var initializer = new DatabaseInitializer(connectionString);
        await initializer.InitializeAsync();
        Log.Information("Banco de dados inicializado com sucesso");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Erro ao inicializar o banco de dados");
        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ApiDapperClean");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();



