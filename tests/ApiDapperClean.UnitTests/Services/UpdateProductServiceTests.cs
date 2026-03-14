using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Services.v1.Products.Update;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class UpdateProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly Mock<IValidator<UpdateProductDto>> _updateValidatorMock;
    private readonly UpdateProductService _service;

    public UpdateProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _updateValidatorMock = new Mock<IValidator<UpdateProductDto>>();
        _service = new UpdateProductService(_repositoryMock.Object, _updateValidatorMock.Object);
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
        result.Data.ShouldNotBeNull();
        result.Data.Name.ShouldBe("Notebook Atualizado");
        result.Data.Price.ShouldBe(2800.00m);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidData_ShouldReturnFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDto { Name = "", Price = -100, Description = "", Stock = -5 };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("Name", "O nome é obrigatório") }
        );

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdateAsync(productId, updateDto);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentProduct_ShouldReturnFailure()
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

        var validationResult = new FluentValidation.Results.ValidationResult();
        _updateValidatorMock
            .Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateAsync(productId, updateDto);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeNull();
        result.Errors.ShouldContain("Produto não encontrado");
    }
}
