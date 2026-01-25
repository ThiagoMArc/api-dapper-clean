namespace ApiDapperClean.Domain.Entities;

/// <summary>
/// Entidade de exemplo - Produto
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Nome do produto
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Preço do produto
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Descrição do produto
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade em estoque
    /// </summary>
    public double Stock { get; set; }
}
