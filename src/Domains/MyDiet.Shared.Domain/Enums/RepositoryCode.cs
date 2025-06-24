using System.Net;

namespace MyDiet.Shared.Domain
{
    public enum RepositoryCode
    {
        Ok = HttpStatusCode.OK,
        Created = HttpStatusCode.Created,
        BadRequest = HttpStatusCode.BadRequest,
        Unauthorized = HttpStatusCode.Unauthorized,
        Forbidden = HttpStatusCode.Forbidden,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError
    }
}
