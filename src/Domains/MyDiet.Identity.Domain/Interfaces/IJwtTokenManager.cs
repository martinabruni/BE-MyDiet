using Microsoft.IdentityModel.Tokens;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtTokenManager<TClaim, TPrivateKey, TPublicKey> where TClaim : class where TPrivateKey : AsymmetricSecurityKey where TPublicKey : class
    {
        Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto);
    }
}
