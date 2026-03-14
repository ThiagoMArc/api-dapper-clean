using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Validators;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Validators;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _createValidator = new();

    [Fact]
    public async Task CreateValidator_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Price = 2500.00m,
            Description = "Notebook de alta performance",
            Stock = 10
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task CreateValidator_WithEmptyName_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto { Name = "", Price = 100m, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task CreateValidator_WithNameTooShort_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto { Name = "AB", Price = 100m, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task CreateValidator_WithNameTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = new string('A', 201),
            Price = 100m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task CreateValidator_WithNegativePrice_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Price = -100m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task CreateValidator_WithZeroPrice_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Price = 0m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task CreateValidator_WithNegativeStock_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Price = 2500m,
            Description = "Descrição",
            Stock = -5
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Stock");
    }

    [Fact]
    public async Task CreateValidator_WithDescriptionTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Price = 2500m,
            Description = new string('A', 1001),
            Stock = 5
        };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
    }

    [Theory]
    [InlineData("Notebook")]
    [InlineData("Mouse Wireless")]
    [InlineData("ABC")]
    public async Task CreateValidator_WithValidNames_ShouldBeValid(string name)
    {
        // Arrange
        var dto = new CreateProductDto { Name = name, Price = 100m, Description = "Descrição válida", Stock = 5 };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(10000.99)]
    public async Task CreateValidator_WithValidPrices_ShouldBeValid(decimal price)
    {
        // Arrange
        var dto = new CreateProductDto { Name = "Produto", Price = price, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(-100.0)]
    public async Task CreateValidator_WithInvalidPrices_ShouldBeInvalid(decimal price)
    {
        // Arrange
        var dto = new CreateProductDto { Name = "Produto", Price = price, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(5.0)]
    [InlineData(1000.0)]
    public async Task CreateValidator_WithValidStock_ShouldBeValid(double stock)
    {
        // Arrange
        var dto = new CreateProductDto { Name = "Produto", Price = 100m, Description = "Descrição", Stock = stock };

        // Act
        var result = await _createValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
