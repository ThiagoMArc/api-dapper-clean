using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Services.v1;
using ApiDapperClean.Application.Services.v1.Products.Create;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class CreateProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly Mock<IValidator<CreateProductDto>> _createValidatorMock;
    private readonly CreateProductService _service;

    public CreateProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _createValidatorMock = new Mock<IValidator<CreateProductDto>>();
        _service = new CreateProductService(_repositoryMock.Object, _createValidatorMock.Object);
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
}
