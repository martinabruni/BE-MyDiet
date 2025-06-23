using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Services
{
    public interface IJwtKeyService<TPrivateKey, TPublicKey> where TPrivateKey : AsymmetricSecurityKey where TPublicKey : class
    {
        Task<ApiResponse<TPrivateKey>> CreateKeyPairAsync();
        Task<ApiResponse<TPrivateKey>> RegenerateKeyPairAsync();
        ApiResponse<TPublicKey> GetPublicKey();
        ApiResponse<TPrivateKey> GetPrivateKey();
        ApiResponse<OpenIdConfigurationDto> GetOpenIdConfigurationKey();
    }
}
