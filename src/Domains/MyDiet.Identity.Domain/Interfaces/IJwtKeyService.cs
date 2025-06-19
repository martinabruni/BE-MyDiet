using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtKeyService<TPrivateKey, TPublicKey> where TPrivateKey : AsymmetricSecurityKey where TPublicKey : class
    {
        Task<ApiResponse<TPrivateKey>> CreatePrivateKeyAsync();
        Task<ApiResponse<TPublicKey>> GetPublicKeyAsync();
        Task<ApiResponse<TPrivateKey>> GetPrivateKeyAsync();
        Task<ApiResponse<OpenIdConfigurationDto>> GetOpenIdConfigurationKeyAsync();
    }
}
