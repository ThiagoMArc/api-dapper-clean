using Npgsql;

namespace ApiDapperClean.Infrastructure.Data;

/// <summary>
/// Gerenciador de conexões com PostgreSQL
/// </summary>
public interface IDbConnection
{
    NpgsqlConnection GetConnection();
}

/// <summary>
/// Implementação do gerenciador de conexões
/// </summary>
public class DbConnection : IDbConnection
{
    private readonly string _connectionString;

    public DbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public NpgsqlConnection GetConnection()
        => new(_connectionString);
}
