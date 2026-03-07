using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using ApiDapperClean.Infrastructure.Data;
using Dapper;

namespace ApiDapperClean.Infrastructure.Repositories;

/// <summary>
/// Repositório genérico para operações com banco de dados usando Dapper
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
public abstract class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IDbConnection _dbConnection;
    protected abstract string TableName { get; }

    public DapperRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnection.GetConnection();
        var sql = $"SELECT * FROM {TableName} WHERE \"Id\" = @Id AND \"IsDeleted\" = false";

        var result = await connection.QueryFirstOrDefaultAsync<TEntity>(
            sql,
            new { Id = id });

        return result;
    }

    public virtual async Task<PagedDataResult<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnection.GetConnection();

        var sqlCount = $"SELECT COUNT(*) FROM {TableName} WHERE \"IsDeleted\" = false";

        var sqlPaged = $"SELECT * FROM {TableName} WHERE \"IsDeleted\" = false ORDER BY \"Id\" OFFSET @Offset ROWS FETCH NEXT @Size ROWS ONLY";

        var sql = $"{sqlCount}; {sqlPaged}";

        var offset = (pageNumber - 1) * pageSize;

        var multi = await connection.QueryMultipleAsync(sql, new { Offset = offset, Size = pageSize });

        return new PagedDataResult<TEntity>
        {
            TotalItems = await multi.ReadSingleAsync<int>(),
            Items = await multi.ReadAsync<TEntity>(),
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        using var connection = _dbConnection.GetConnection();
        var properties = GetInsertProperties();
        var columns = string.Join(", ", properties.Select(p => $"\"{p}\""));
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({values})";

        await connection.ExecuteAsync(sql, entity);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        using var connection = _dbConnection.GetConnection();
        var properties = GetUpdateProperties();
        var setClause = string.Join(", ", properties.Select(p => $"\"{p}\" = @{p}"));

        var sql = $"UPDATE {TableName} SET {setClause} WHERE \"Id\" = @Id";

        await connection.ExecuteAsync(sql, entity);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnection.GetConnection();
        var sql = $"UPDATE {TableName} SET \"IsDeleted\" = true, \"UpdatedAt\" = @UpdatedAt WHERE \"Id\" = @Id";

        await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.UtcNow });
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dapper não mantém mudanças em memória, então retornamos 0
        // Este método é mantido para compatibilidade com a interface IRepository
        return await Task.FromResult(0);
    }

    protected virtual List<string> GetInsertProperties()
    {
        return typeof(TEntity).GetProperties()
            .Where(p => p.CanRead && p.CanWrite)
            .Select(p => p.Name)
            .ToList();
    }

    protected virtual List<string> GetUpdateProperties()
    {
        return typeof(TEntity).GetProperties()
            .Where(p => p.CanRead && p.CanWrite && p.Name != "Id" && p.Name != "CreatedAt")
            .Select(p => p.Name)
            .ToList();
    }
}
