using MyDiet.Shared.Domain.Responses;
using System.Net;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtension
    {
        public static IActionResult ComposeResult<TData>(this ControllerBase controllerBase, ApiResponse<TData> apiResponse) where TData : class
        {
            return apiResponse.StatusCode switch
            {
                HttpStatusCode.OK => controllerBase.Ok(apiResponse),
                HttpStatusCode.Created => controllerBase.Created(string.Empty, apiResponse),
                HttpStatusCode.NoContent => controllerBase.NoContent(),
                HttpStatusCode.Conflict => controllerBase.Conflict(apiResponse),
                HttpStatusCode.NotFound => controllerBase.NotFound(apiResponse),
                HttpStatusCode.Unauthorized => controllerBase.Unauthorized(apiResponse),
                HttpStatusCode.InternalServerError => controllerBase.StatusCode(500, apiResponse),
                HttpStatusCode.PartialContent => controllerBase.StatusCode(206, null),
                _ => controllerBase.StatusCode(500, apiResponse)
            };
        }
    }
}
