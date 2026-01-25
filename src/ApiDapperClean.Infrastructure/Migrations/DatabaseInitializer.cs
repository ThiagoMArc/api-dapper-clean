using Npgsql;

namespace ApiDapperClean.Infrastructure.Migrations;

/// <summary>
/// Gerenciador de migrations para PostgreSQL
/// </summary>
public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Inicializa o banco de dados criando as tabelas necessárias
    /// </summary>
    public async Task InitializeAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // Criar extensão UUID se não existir
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"";
            await cmd.ExecuteNonQueryAsync();
        }

        // Criar tabela Products
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ""Products""
                (
                    ""Id"" uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
                    ""Name"" varchar(200) NOT NULL,
                    ""Price"" numeric(10,2) NOT NULL,
                    ""Description"" varchar(1000),
                    ""Stock"" double precision NOT NULL DEFAULT 0,
                    ""CreatedAt"" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""UpdatedAt"" timestamp,
                    ""IsDeleted"" boolean NOT NULL DEFAULT false
                )
            ";
            await cmd.ExecuteNonQueryAsync();
        }

        // Criar índices
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE INDEX IF NOT EXISTS idx_products_isdeleted ON ""Products""(""IsDeleted"");
                CREATE INDEX IF NOT EXISTS idx_products_name ON ""Products""(""Name"");
            ";
            await cmd.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }

    /// <summary>
    /// Limpa o banco de dados (apenas para desenvolvimento)
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "DROP TABLE IF EXISTS \"Products\" CASCADE";
            await cmd.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }
}
