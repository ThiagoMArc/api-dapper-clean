using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services.v1;

/// <summary>
/// Interface de serviço para Product
/// </summary>
public interface IProductService
{
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PagedDataResult<ProductDto>>> GetPagedAsync(GetProductsDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


