using ApiDapperClean.Application.DTOs.Product;
using ApiDapperClean.Application.Validators;
using Shouldly;
using Xunit;

namespace ApiDapperClean.UnitTests.Validators;

public class UpdateProductValidatorTests
{
    private readonly UpdateProductValidator _updateValidator = new();

    [Fact]
    public async Task UpdateValidator_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = 2500.00m,
            Description = "Notebook de alta performance",
            Stock = 10
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task UpdateValidator_WithEmptyName_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "", Price = 100m, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task UpdateValidator_WithNameTooShort_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "AB", Price = 100m, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task UpdateValidator_WithNameTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = new string('A', 201),
            Price = 100m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task UpdateValidator_WithNegativePrice_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = -100m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task UpdateValidator_WithZeroPrice_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = 0m,
            Description = "Descrição",
            Stock = 5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task UpdateValidator_WithNegativeStock_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = 2500m,
            Description = "Descrição",
            Stock = -5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Stock");
    }

    [Fact]
    public async Task UpdateValidator_WithDescriptionTooShort_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = 2500m,
            Description = "ABCD",
            Stock = 5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task UpdateValidator_WithDescriptionTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Notebook",
            Price = 2500m,
            Description = new string('A', 1001),
            Stock = 5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
    }

    [Theory]
    [InlineData("Notebook")]
    [InlineData("Mouse Wireless")]
    [InlineData("ABC")]
    public async Task UpdateValidator_WithValidNames_ShouldBeValid(string name)
    {
        // Arrange
        var dto = new UpdateProductDto { Name = name, Price = 100m, Description = "Descrição válida", Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(10000.99)]
    public async Task UpdateValidator_WithValidPrices_ShouldBeValid(decimal price)
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "Produto", Price = price, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(-100.0)]
    public async Task UpdateValidator_WithInvalidPrices_ShouldBeInvalid(decimal price)
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "Produto", Price = price, Description = "Descrição", Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(1000)]
    public async Task UpdateValidator_WithValidStockValues_ShouldBeValid(double stock)
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "Produto", Price = 100m, Description = "Descrição", Stock = stock };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("Descrição válida")]
    [InlineData("ABCDE")]
    [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.")]
    public async Task UpdateValidator_WithValidDescriptions_ShouldBeValid(string description)
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "Produto", Price = 100m, Description = description, Stock = 5 };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WithMultipleErrors_ShouldHaveAllErrors()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "AB",
            Price = -100m,
            Description = "AB",
            Stock = -5
        };

        // Act
        var result = await _updateValidator.ValidateAsync(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(4);
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
        result.Errors.ShouldContain(e => e.PropertyName == "Price");
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
        result.Errors.ShouldContain(e => e.PropertyName == "Stock");
    }
}
