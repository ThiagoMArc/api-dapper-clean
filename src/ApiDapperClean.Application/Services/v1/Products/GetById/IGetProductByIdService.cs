using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Domain.Results;


namespace ApiDapperClean.Application.Services.v1.Products.GetById;

public interface IGetProductByIdService
{
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
