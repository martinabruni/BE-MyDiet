namespace MyDiet.Session.Domain.Responses
{
    public class RepositoryResponse<TData>
    {
        public TData? Data { get; set; }
        public string? Message { get; set; }
        public required RepositoryCode StatusCode { get; set; }
    }
}
