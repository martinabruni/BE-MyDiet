using System.Security.Claims;

namespace MyDiet.Core.Domain.Managers
{
    public interface IValidationManager<TRequest>
        where TRequest : class
    {
        abstract Guid? ValidateUserClaim(Claim? claim);
        abstract TRequest? ValidateRequest(TRequest request);
        abstract Guid? ValidateAndGetUserId(TRequest request, Claim? claim);
    }
}
