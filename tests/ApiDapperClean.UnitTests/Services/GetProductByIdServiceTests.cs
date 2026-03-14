using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Services.v1;
using ApiDapperClean.Application.Services.v1.Products.GetById;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class GetProductByIdServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly GetProductByIdService _service;

    public GetProductByIdServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _service = new GetProductByIdService(_repositoryMock.Object);
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
        result.Data.ShouldNotBeNull();
        result.Data.Id.ShouldBe(productId);
        result.Data.Name.ShouldBe("Notebook");
        result.Data.Price.ShouldBe(2500.00m);
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
        result.Data.ShouldBeNull();
    }
}
