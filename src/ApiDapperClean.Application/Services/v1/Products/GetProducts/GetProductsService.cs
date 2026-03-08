using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using ApiDapperClean.Domain.Results;
using FluentValidation;
using Serilog;


namespace ApiDapperClean.Application.Services.v1.Products.GetProducts;

public class GetProductsService : IGetProductsService
{
    private readonly IRepository<Product> _repository;
    private readonly IValidator<GetProductsDto> _validator;

    public GetProductsService(IRepository<Product> repository, IValidator<GetProductsDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<PagedDataResult<ProductDto>>> GetPagedAsync(GetProductsDto dto, CancellationToken cancellationToken = default)
    {
        Log.Information("Recuperando todos os produtos");

        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<PagedDataResult<ProductDto>>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), null);
        }

        PagedDataResult<Product>? pagedDataProduct = await _repository.GetPagedAsync(dto.Page, dto.PageSize, cancellationToken);
        PagedDataResult<ProductDto> pagedDataDto = ProductMapper.ToPagedDataDto(pagedDataProduct);

        Log.Information("Total de {Count} produtos recuperados", pagedDataProduct.TotalItems);

        return Result<PagedDataResult<ProductDto>>.Success(pagedDataDto);
    }
}
