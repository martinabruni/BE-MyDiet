namespace MyDiet.Shared.Domain.Responses
{
    public class ApiDataResponse<TData> : ApiResponse
        where TData : class
    {
        public TData Data { get; set; } = default!;
    }
}
