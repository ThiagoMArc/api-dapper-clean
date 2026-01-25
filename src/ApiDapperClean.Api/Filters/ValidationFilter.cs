using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDapperClean.Api.Filters;

/// <summary>
/// Filtro de validação automática de modelos
/// </summary>
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            context.Result = new BadRequestObjectResult(new
            {
                errors = errors,
                message = "Erro de validação"
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
