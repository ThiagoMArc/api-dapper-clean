using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using FluentValidation;
using Serilog;


namespace ApiDapperClean.Application.Services.v1.Products.Update;

public class UpdateProductService : IUpdateProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public UpdateProductService(IRepository<Product> repository, IValidator<UpdateProductDto> updateValidator)
    {
        _repository = repository;
        _updateValidator = updateValidator;
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Atualizando produto com ID: {ProductId}", id);

        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<ProductDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), null);
        }

        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            Log.Warning("Produto com ID {ProductId} não encontrado para atualização", id);
            return Result<ProductDto>.Failure(["Produto não encontrado"]);
        }

        ProductMapper.UpdateEntity(product, dto);
        await _repository.UpdateAsync(product, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        Log.Information("Produto {ProductId} atualizado com sucesso", id);

        var resultDto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(resultDto);
    }
}
