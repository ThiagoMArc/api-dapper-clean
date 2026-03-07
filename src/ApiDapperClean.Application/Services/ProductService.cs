using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using FluentValidation;
using Serilog;

namespace ApiDapperClean.Application.Services;

/// <summary>
/// Implementação do serviço de Product
/// </summary>
public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductService(
        IRepository<Product> repository,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Log.Information("Recuperando produto com ID: {ProductId}", id);

        var product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product == null)
        {
            Log.Warning("Produto com ID {ProductId} não encontrado", id);
            return Result<ProductDto>.Failure("Produto não encontrado");
        }

        var dto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(dto);
    }

    public async Task<Result<PagedDataResult<ProductDto>>> GetPagedAsync(GetProductsDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Recuperando todos os produtos");

        PagedDataResult<Product>? pagedDataProduct = await _repository.GetPagedAsync(dto.Page, dto.PageSize, cancellationToken);
        PagedDataResult<ProductDto> pagedDataDto = ProductMapper.ToPagedDataDto(pagedDataProduct);

        Log.Information("Total de {Count} produtos recuperados", pagedDataProduct.TotalItems);

        return Result<PagedDataResult<ProductDto>>.Success(pagedDataDto);
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Criando novo produto: {ProductName}", dto.Name);

        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validação falhou ao criar produto: {Error}", validationResult.Errors.First().ErrorMessage);
            return Result<ProductDto>.Failure(validationResult.Errors.First().ErrorMessage);
        }

        var product = ProductMapper.ToEntity(dto);
        await _repository.AddAsync(product, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        Log.Information("Produto criado com sucesso. ID: {ProductId}", product.Id);

        var resultDto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(resultDto);
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Atualizando produto com ID: {ProductId}", id);

        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validação falhou ao atualizar produto {ProductId}: {Error}", id, validationResult.Errors.First().ErrorMessage);
            return Result<ProductDto>.Failure(validationResult.Errors.First().ErrorMessage);
        }

        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            Log.Warning("Produto com ID {ProductId} não encontrado para atualização", id);
            return Result<ProductDto>.Failure("Produto não encontrado");
        }

        ProductMapper.UpdateEntity(product, dto);
        await _repository.UpdateAsync(product, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        Log.Information("Produto {ProductId} atualizado com sucesso", id);

        var resultDto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(resultDto);
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
