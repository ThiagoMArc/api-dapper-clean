namespace ApiDapperClean.Application.DTOs.Product;

/// <summary>
/// DTO para atualização de produto
/// </summary>
public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Stock { get; set; }
}