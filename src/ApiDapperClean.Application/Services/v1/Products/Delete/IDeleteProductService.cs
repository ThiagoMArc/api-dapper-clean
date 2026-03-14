using ApiDapperClean.Domain.Results;

namespace ApiDapperClean.Application.Services.v1.Products.Delete;

public interface IDeleteProductService
{
    Task<Result<bool?>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
