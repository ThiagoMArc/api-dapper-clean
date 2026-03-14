using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using FluentValidation;
using Serilog;
using System.Net;


namespace ApiDapperClean.Application.Services.v1.Products.Create;

public class CreateProductService : ICreateProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IValidator<CreateProductDto> _createValidator;

    public CreateProductService(IRepository<Product> repository, IValidator<CreateProductDto> createValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Criando novo produto: {ProductName}", dto.Name);

        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result<ProductDto>.Failure(errorMessages, HttpStatusCode.BadRequest);
        }

        var product = ProductMapper.ToEntity(dto);
        await _repository.AddAsync(product, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        Log.Information("Produto criado com sucesso. ID: {ProductId}", product.Id);

        var resultDto = ProductMapper.ToDto(product);
        return Result<ProductDto>.Success(resultDto, HttpStatusCode.Created);
    }
}
