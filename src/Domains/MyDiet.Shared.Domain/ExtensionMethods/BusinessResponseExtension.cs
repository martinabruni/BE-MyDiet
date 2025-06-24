using Microsoft.AspNetCore.Mvc;

namespace MyDiet.Shared.Domain
{
    public static class BusinessResponseExtension
    {
        public static IActionResult ToActionResult<TData>(this BusinessResponse<TData> response)
            where TData : class
        {
            return response.StatusCode switch
            {
                BusinessCode.Ok => new OkObjectResult(response),
                BusinessCode.Created => new CreatedResult(string.Empty, response),
                BusinessCode.BadRequest => new BadRequestObjectResult(response),
                BusinessCode.Unauthorized => new UnauthorizedResult(),
                BusinessCode.Forbidden => new ForbidResult(),
                BusinessCode.NotFound => new NotFoundObjectResult(response),
                BusinessCode.InternalServerError => new ObjectResult(response) { StatusCode = 500 },
                _ => new ObjectResult(response) { StatusCode = 500 }
            };
        }
    }
}
