using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services;

/// <summary>
/// Interface de serviço para Product
/// </summary>
public interface IProductService
{
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


