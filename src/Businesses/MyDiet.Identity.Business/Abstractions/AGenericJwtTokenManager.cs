using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Shared.Domain.Responses;
using System.Net;

namespace MyDiet.Identity.Business.Abstractions
{
    internal abstract class AGenericJwtTokenManager<TClaim, TPrivateKey, TPublicKey> : IJwtTokenManager<TClaim, TPrivateKey, TPublicKey> where TClaim : class where TPrivateKey : AsymmetricSecurityKey where TPublicKey : class
    {
        private readonly IJwtKeyService<TPrivateKey, TPublicKey> _jwtKeyService;
        private readonly IJwtTokenService<TClaim, TPrivateKey> _jwtTokenService;

        public AGenericJwtTokenManager(IJwtTokenService<TClaim, TPrivateKey> jwtTokenService, IJwtKeyService<TPrivateKey, TPublicKey> jwtKeyService)
        {
            _jwtTokenService = jwtTokenService;
            _jwtKeyService = jwtKeyService;
        }

        public virtual async Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto)
        {
            var privateKeyRes = await _jwtKeyService.GetPrivateKeyAsync();
            if (privateKeyRes.Data is null)
            {
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Private key not found in Key Vault."
                };
            }
            return await _jwtTokenService.GenerateTokenAsync(claimDto, privateKeyRes.Data);
        }
    }
}
