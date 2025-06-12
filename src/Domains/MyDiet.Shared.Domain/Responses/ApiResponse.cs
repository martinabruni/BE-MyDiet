using System.Net;

namespace MyDiet.Shared.Domain.Responses
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; } = "";
    }
}
