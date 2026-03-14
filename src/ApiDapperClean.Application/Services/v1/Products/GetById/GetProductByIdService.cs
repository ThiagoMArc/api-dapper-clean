using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using Serilog;
using System.Net;

namespace ApiDapperClean.Application.Services.v1.Products.GetById;

public class GetProductByIdService : IGetProductByIdService
{
    private readonly IRepository<Product> _repository;

    public GetProductByIdService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Log.Information("Recuperando produto com ID: {ProductId}", id);

        var product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product == null)
        {
            return Result<ProductDto>.Failure(["Produto não encontrado"], HttpStatusCode.NotFound);
        }

        var dto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(dto, HttpStatusCode.OK);
    }
}
