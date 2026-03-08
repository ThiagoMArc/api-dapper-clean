using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using Serilog;


namespace ApiDapperClean.Application.Services.v1.Products.Delete;

public class DeleteProductService : IDeleteProductService
{
    private readonly IRepository<Product> _repository;

    public DeleteProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Log.Information("Deletando produto com ID: {ProductId}", id);

        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            Log.Warning("Produto com ID {ProductId} não encontrado para deleção", id);
            return Result.Failure("Produto não encontrado");
        }

        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        Log.Information("Produto {ProductId} deletado com sucesso", id);

        return Result.Success();
    }
}
