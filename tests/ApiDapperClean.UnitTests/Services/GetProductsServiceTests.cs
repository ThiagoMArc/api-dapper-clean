using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Services.v1.Products.GetProducts;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class GetProductsServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly Mock<IValidator<GetProductsDto>> _validatorMock;
    private readonly GetProductsService _service;

    public GetProductsServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _validatorMock = new Mock<IValidator<GetProductsDto>>();
        _service = new GetProductsService(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetPagedAsync_WithValidParameters_ShouldReturnSuccess()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Notebook", Price = 2500.00m, Description = "Notebook", Stock = 10 },
            new Product { Id = Guid.NewGuid(), Name = "Mouse", Price = 50.00m, Description = "Mouse", Stock = 50 },
            new Product { Id = Guid.NewGuid(), Name = "Teclado", Price = 150.00m, Description = "Teclado", Stock = 30 }
        };

        var pagedResult = new ApiDapperClean.Domain.Results.PagedDataResult<Product>
        {
            Items = products,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = 3
        };

        var getProductsDto = new GetProductsDto { Page = page, PageSize = pageSize };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(getProductsDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _repositoryMock
            .Setup(r => r.GetPagedAsync(page, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _service.GetPagedAsync(getProductsDto);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Items.Count().ShouldBe(3);
        result.Value.PageNumber.ShouldBe(page);
        result.Value.PageSize.ShouldBe(pageSize);
        result.Value.TotalItems.ShouldBe(3);
        result.Value.Items.First().Name.ShouldBe("Notebook");
    }

    [Fact]
    public async Task GetPagedAsync_WithEmptyPage_ShouldReturnSuccess()
    {
        // Arrange
        var page = 5;
        var pageSize = 10;

        var pagedResult = new ApiDapperClean.Domain.Results.PagedDataResult<Product>
        {
            Items = new List<Product>(),
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = 0
        };

        var getProductsDto = new GetProductsDto { Page = page, PageSize = pageSize };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(getProductsDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _repositoryMock
            .Setup(r => r.GetPagedAsync(page, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _service.GetPagedAsync(getProductsDto);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Items.Count().ShouldBe(0);
        result.Value.TotalItems.ShouldBe(0);
    }

    [Fact]
    public async Task GetPagedAsync_WithInvalidParameters_ShouldReturnFailure()
    {
        // Arrange
        var getProductsDto = new GetProductsDto { Page = 0, PageSize = 0 };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("Page", "A página deve ser maior que 0") }
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(getProductsDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.GetPagedAsync(getProductsDto);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }
}
