using BaseUtility;
using System.Security.Claims;

namespace MyDiet.Core.Domain.Managers
{
    public interface IManager<TResponse, TRequest, TKey>
        where TResponse : class
        where TRequest : class
        where TKey : notnull
    {
        Task<BusinessResponse<TResponse>> GetByIdAsync(TKey id, Claim? userIdClaim);

        Task<BusinessResponse<TResponse>> CreateAsync(TRequest request, Claim? userIdClaim);

        Task<BusinessResponse<TResponse>> UpdateAsync(TRequest request, TKey id, Claim? userIdClaim);

        Task<BusinessResponse<TResponse>> DeleteAsync(TKey id, Claim? userIdClaim);

        Task<BusinessResponse<IEnumerable<TResponse>>> GetByUserIdAsync(Claim? userIdClaim);
    }
}
