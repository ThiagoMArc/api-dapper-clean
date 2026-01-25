using Serilog;
using ApiDapperClean.CrossCutting.IoC;
using ApiDapperClean.Api.Middlewares;
using ApiDapperClean.Api.Filters;
using ApiDapperClean.Infrastructure.Migrations;
using Scalar.AspNetCore;
using Serilog.Events;

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

// Registrar dependências
builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddInfrastructure(connectionString);

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



