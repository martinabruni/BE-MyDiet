using System.Net;

namespace MyDiet.Shared.Domain.Responses
{
    public class ApiResponse<TData> where TData : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public TData? Data { get; set; }
    }
}
