using FluentValidation;
using ApiDapperClean.Application.DTOs.Product;

namespace ApiDapperClean.Application.Validators;

public class GetProductsValidator : AbstractValidator<GetProductsDto>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("A página deve ser maior que zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("O tamanho da página deve ser maior que zero")
            .LessThanOrEqualTo(100).WithMessage("O tamanho da página deve ser no máximo 100");
    }
}
