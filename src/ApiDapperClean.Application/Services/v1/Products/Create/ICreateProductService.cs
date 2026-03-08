using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services.v1.Products.Create;

public interface ICreateProductService
{
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
}
