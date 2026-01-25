using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Infrastructure.Data;

namespace ApiDapperClean.Infrastructure.Repositories;

/// <summary>
/// Repositório específico para Product
/// </summary>
public class ProductRepository : DapperRepository<Product>
{
    protected override string TableName => "\"Products\"";

    public ProductRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }
}
