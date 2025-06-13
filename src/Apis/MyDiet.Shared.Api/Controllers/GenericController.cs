using Microsoft.AspNetCore.Mvc;
using MyDiet.Shared.Domain.Responses;
using System.Net;

public abstract class GenericController : ControllerBase
{
    protected virtual IActionResult ComposeResult(ApiResponse res)
    {
        return res.StatusCode switch
        {
            HttpStatusCode.OK => Ok(res.Message),
            HttpStatusCode.Created => Created(string.Empty, res.Message),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.Conflict => Conflict(res),
            HttpStatusCode.NotFound => NotFound(res),
            HttpStatusCode.Unauthorized => Unauthorized(res),
            HttpStatusCode.InternalServerError => StatusCode(500, res),
            HttpStatusCode.PartialContent => StatusCode(206, null),
            _ => StatusCode(500, res)
        };
    }

    protected virtual IActionResult ComposeResult<TData>(ApiDataResponse<TData> res) where TData : class
    {
        return res.StatusCode switch
        {
            HttpStatusCode.OK => Ok(res.Data),
            HttpStatusCode.Created => Created(string.Empty, res.Data),
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