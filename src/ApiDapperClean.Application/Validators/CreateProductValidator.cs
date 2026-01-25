using ApiDapperClean.Application.DTOs.Product;
using FluentValidation;

namespace ApiDapperClean.Application.Validators;

/// <summary>
/// Validador para CreateProductDto
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo");
    }
}