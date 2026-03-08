using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services.v1.Products.Update;

public interface IUpdateProductService
{
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
}
