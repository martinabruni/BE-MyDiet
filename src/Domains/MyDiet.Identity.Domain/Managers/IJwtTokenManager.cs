using Microsoft.IdentityModel.Tokens;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Managers
{
    public interface IJwtTokenManager<TDto, TClaim, TPrivateKey, TPublicKey>
        where TDto : class
        where TClaim : class
    {
        Task<ApiResponse<string>> GenerateTokenAsync(TDto dto);
        Task<TClaim?> FindClaimsAsync(TDto dto);
    }
}
