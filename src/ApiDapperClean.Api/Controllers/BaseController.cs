using Microsoft.AspNetCore.Mvc;
using ApiDapperClean.Domain.Results;
using System.Net;

namespace ApiDapperClean.Api.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult GenerateResponse<T>(Result<T> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => Ok(result),
            HttpStatusCode.Created => Created("", result),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.NotFound => NotFound(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
