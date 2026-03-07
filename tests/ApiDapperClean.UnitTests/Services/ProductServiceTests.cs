using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Mappers;
using ApiDapperClean.Application.Services;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly Mock<IValidator<CreateProductDto>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateProductDto>> _updateValidatorMock;
    private readonly Mock<IValidator<GetProductsDto>> _getProductsValidatorMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _createValidatorMock = new Mock<IValidator<CreateProductDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateProductDto>>();
        _getProductsValidatorMock = new Mock<IValidator<GetProductsDto>>();

        _service = new ProductService(_repositoryMock.Object,
                                       _createValidatorMock.Object,
                                       _updateValidatorMock.Object,
                                        _getProductsValidatorMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Notebook",
            Price = 2500.00m,
            Description = "Notebook de alta performance",
            Stock = 10,
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(productId);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Id.ShouldBe(productId);
        result.Value.Name.ShouldBe("Notebook");
        result.Value.Price.ShouldBe(2500.00m);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(productId);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "Notebook",
            Price = 2500.00m,
            Description = "Notebook de alta performance",
            Stock = 10
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _createValidatorMock
            .Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Name.ShouldBe("Notebook");
        result.Value.Price.ShouldBe(2500.00m);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidData_ShouldReturnFailure()
    {
        // Arrange
        var createDto = new CreateProductDto { Name = "", Price = -100, Description = "", Stock = -5 };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("Name", "O nome é obrigatório") }
        );

        _createValidatorMock
            .Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDto
        {
            Name = "Notebook Atualizado",
            Price = 2800.00m,
            Description = "Notebook com processador mais potente",
            Stock = 15
        };

        var product = new Product
        {
            Id = productId,
            Name = "Notebook",
            Price = 2500.00m,
            Description = "Notebook de alta performance",
            Stock = 10
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _updateValidatorMock
            .Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.UpdateAsync(productId, updateDto);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Name.ShouldBe("Notebook Atualizado");
        result.Value.Price.ShouldBe(2800.00m);
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
        _getProductsValidatorMock
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
        _getProductsValidatorMock
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
    public async Task DeleteAsync_WhenProductExists_ShouldReturnSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Notebook", Price = 2500.00m, Stock = 10 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _repositoryMock
            .Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.DeleteAsync(productId);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
}
