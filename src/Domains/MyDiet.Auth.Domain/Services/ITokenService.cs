using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Responses;

namespace MyDiet.Auth.Domain.Services
{
    public interface ITokenService
    {
        BusinessResponse<TokenResponse> GenerateToken(UserClaims claimDto, KeyVaultSecret privateKey);
        Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token);
    }
}
