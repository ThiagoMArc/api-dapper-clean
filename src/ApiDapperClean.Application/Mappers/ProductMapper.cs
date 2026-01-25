using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Entities;

namespace ApiDapperClean.Application.Mappers;

/// <summary>
/// Mapeador estático para conversões entre Product e seus DTOs
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Converte uma entidade Product para ProductDto
    /// </summary>
    public static ProductDto ToDto(Product product)
    {
        if (product == null) return null!;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Stock = product.Stock,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    /// <summary>
    /// Converte um CreateProductDto para uma entidade Product
    /// </summary>
    public static Product ToEntity(CreateProductDto dto)
    {
        if (dto == null) return null!;

        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            Stock = dto.Stock
        };
    }

    /// <summary>
    /// Atualiza uma entidade Product com dados de UpdateProductDto
    /// </summary>
    public static void UpdateEntity(Product product, UpdateProductDto dto)
    {
        if (product == null || dto == null) return;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.Stock = dto.Stock;
        product.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Converte uma coleção de Product para coleção de ProductDto
    /// </summary>
    public static IEnumerable<ProductDto> ToDtoList(IEnumerable<Product> products)
    {
        return products?.Select(ToDto) ?? Enumerable.Empty<ProductDto>();
    }
}
