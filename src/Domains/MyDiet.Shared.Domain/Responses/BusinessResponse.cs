namespace MyDiet.Shared.Domain
{
    public class BusinessResponse<TData>
        where TData : class
    {
        public TData? Data { get; set; }
        public string? Message { get; set; }
        public required BusinessCode StatusCode { get; set; }
    }
}
