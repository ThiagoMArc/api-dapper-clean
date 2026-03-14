using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Validators;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Validators;

public class GetProductsValidatorTests
{
    private readonly GetProductsValidator _getProductsValidator = new();

    [Fact]
    public async Task GetProductsValidator_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new GetProductsDto { Page = 1, PageSize = 10 };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task GetProductsValidator_WithValidPages_ShouldBeValid(int page)
    {
        // Arrange
        var dto = new GetProductsDto { Page = page, PageSize = 10 };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task GetProductsValidator_WithInvalidPages_ShouldBeInvalid(int page)
    {
        // Arrange
        var dto = new GetProductsDto { Page = page, PageSize = 10 };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Page");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task GetProductsValidator_WithValidPageSizes_ShouldBeValid(int pageSize)
    {
        // Arrange
        var dto = new GetProductsDto { Page = 1, PageSize = pageSize };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    [InlineData(101)]
    [InlineData(200)]
    [InlineData(1000)]
    public async Task GetProductsValidator_WithInvalidPageSizes_ShouldBeInvalid(int pageSize)
    {
        // Arrange
        var dto = new GetProductsDto { Page = 1, PageSize = pageSize };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "PageSize");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-10, 200)]
    public async Task GetProductsValidator_WithMultipleErrors_ShouldHaveAllErrors(int page, int pageSize)
    {
        // Arrange
        var dto = new GetProductsDto { Page = page, PageSize = pageSize };

        // Act
        var result = await _getProductsValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(e => e.PropertyName == "Page");
        result.Errors.ShouldContain(e => e.PropertyName == "PageSize");
    }
}
