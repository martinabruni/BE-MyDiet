using MyDiet.Core.Domain.Managers;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal abstract class BaseManager<TRequest> : IValidationManager<TRequest>
        where TRequest : class
    {
        public Guid? ValidateUserClaim(Claim? claim)
        {
            if (claim is null || !Guid.TryParse(claim.Value, out var userId))
            {
                return null;
            }
            return userId;
        }

        public TRequest? ValidateRequest(TRequest request)
        {
            if (request is null)
            {
                return null;
            }
            return request;
        }

        public virtual Guid? ValidateAndGetUserId(TRequest request, Claim? claim)
        {
            var validRequest = ValidateRequest(request);
            if (validRequest is null)
            {
                return null;
            }
            var userId = ValidateUserClaim(claim);
            if (userId is null)
            {
                return null;
            }
            return userId;
        }
    }
}
