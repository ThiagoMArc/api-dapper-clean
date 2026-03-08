using ApiDapperClean.Application.Services.v1.Products.Delete;
using ApiDapperClean.Domain.Entities;
using ApiDapperClean.Domain.Interfaces;
using Moq;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Services;

public class DeleteProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly DeleteProductService _service;

    public DeleteProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _service = new DeleteProductService(_repositoryMock.Object);
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

    [Fact]
    public async Task DeleteAsync_WhenProductNotExists_ShouldReturnFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.DeleteAsync(productId);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Produto não encontrado");
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDeleteMethod()
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
        await _service.DeleteAsync(productId);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
