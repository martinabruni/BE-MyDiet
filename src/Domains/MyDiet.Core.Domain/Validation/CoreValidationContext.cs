using System.Security.Claims;

namespace MyDiet.Core.Domain.Validation
{
    public class CoreValidationContext<TData, TKey> : IContext<TData>
        where TData : class
        where TKey : notnull
    {
        public TData? Data { get; set; }
        public TData? OldData {  get; set; }
        public Guid UserId { get; set; }
        public Claim? UserClaim { get; set; }
    }
}
