using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Responses;

namespace MyDiet.Auth.Domain.Managers
{
    public interface ITokenManager
    {
        Task<BusinessResponse<TokenResponse>> GenerateTokenAsync(UserClaims claims);
        Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token);
        Task<BusinessResponse<TokenValidationParameters>> GetValidationParametersAsync();
    }
}
