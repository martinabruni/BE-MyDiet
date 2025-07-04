using Microsoft.IdentityModel.Tokens;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Services
{
    public interface IJwtTokenService<TClaim, TPrivateKey>
        where TClaim : class
        where TPrivateKey : AsymmetricSecurityKey
    {
        Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto, TPrivateKey privateKey);
    }
}
