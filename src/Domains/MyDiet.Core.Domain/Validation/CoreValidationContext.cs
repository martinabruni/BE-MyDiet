using System.Security.Claims;

namespace MyDiet.Core.Domain.Validation
{
    public class CoreValidationContext<TData, TKey>
        where TData : class
        where TKey : notnull
    {
        public TData? Data { get; set; }
        public Guid UserId { get; set; }
        public TKey? EntityId { get; set; }
        public Claim? UserClaim { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
    }
}
