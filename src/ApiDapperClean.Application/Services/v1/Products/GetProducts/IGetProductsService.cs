using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services.v1.Products.GetProducts;

public interface IGetProductsService
{
    Task<Result<PagedDataResult<ProductDto>>> GetPagedAsync(GetProductsDto dto, CancellationToken cancellationToken = default);
}
