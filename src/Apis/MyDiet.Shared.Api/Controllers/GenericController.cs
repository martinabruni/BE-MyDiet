using Microsoft.AspNetCore.Mvc;
using MyDiet.Shared.Domain.Responses;
using System.Net;

public abstract class GenericController : ControllerBase
{
    protected virtual IActionResult ComposeResult<TData>(ApiResponse<TData> res) where TData : class
    {
        return res.StatusCode switch
        {
            HttpStatusCode.OK => Ok(res),
            HttpStatusCode.Created => Created(string.Empty, res),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.Conflict => Conflict(res),
            HttpStatusCode.NotFound => NotFound(res),
            HttpStatusCode.Unauthorized => Unauthorized(res),
            HttpStatusCode.InternalServerError => StatusCode(500, res),
            HttpStatusCode.PartialContent => StatusCode(206, null),
            _ => StatusCode(500, res)
        };
    }
}