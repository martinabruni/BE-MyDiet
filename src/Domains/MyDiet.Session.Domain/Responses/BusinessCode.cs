using System.Net;

namespace MyDiet.Session.Domain.Responses
{
    public enum BusinessCode
    {
        Success = HttpStatusCode.OK,
        Created = HttpStatusCode.Created,
        BadRequest = HttpStatusCode.BadRequest,
        Unauthorized = HttpStatusCode.Unauthorized,
        Forbidden = HttpStatusCode.Forbidden,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,
        NotImplemented = HttpStatusCode.NotImplemented,
    }
}