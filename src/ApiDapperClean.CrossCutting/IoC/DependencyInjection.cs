using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ApiDapperClean.Application.Services;
using ApiDapperClean.Application.Validators;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Infrastructure.Data;
using ApiDapperClean.Infrastructure.Repositories;

namespace ApiDapperClean.CrossCutting.IoC;

/// <summary>
/// Extensão para registrar as dependências da aplicação
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada de aplicação
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar validadores
        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

        // Registrar serviços
        services.AddScoped<IProductService, ProductService>();

        return services;
    }

    /// <summary>
    /// Registra os serviços da camada de infraestrutura
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Registrar DbConnection
        services.AddScoped<IDbConnection>(_ => new DbConnection(connectionString));

        // Registrar repositórios
        services.AddScoped<IRepository<Product>, ProductRepository>();

        return services;
    }
}
