namespace ApiDapperClean.Domain.Interfaces;

/// <summary>
/// Interface genérica de repositório
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Obtém uma entidade por ID
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva as alterações no banco de dados
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
