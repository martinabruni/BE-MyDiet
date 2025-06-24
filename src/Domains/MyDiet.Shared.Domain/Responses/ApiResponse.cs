using System.Net;

namespace MyDiet.Shared.Domain
{
    public class ApiResponse<TData>
    {
        public TData? Data { get; set; }
        public string? Message { get; set; }
        public required HttpStatusCode StatusCode { get; set; }
    }
}
