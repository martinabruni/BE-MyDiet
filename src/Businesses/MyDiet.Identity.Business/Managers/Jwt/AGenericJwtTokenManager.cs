using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Managers;
using MyDiet.Identity.Domain.Services;
using MyDiet.Shared.Domain.Responses;
using System.Net;

namespace MyDiet.Identity.Business.Managers.Jwt
{
    internal abstract class AGenericJwtTokenManager<TDto, TClaim, TPrivateKey, TPublicKey> : IJwtTokenManager<TDto, TClaim, TPrivateKey, TPublicKey>
    where TDto : class
    where TClaim : class
    where TPublicKey : class
    where TPrivateKey : AsymmetricSecurityKey
    {
        private readonly IJwtKeyService<TPrivateKey, TPublicKey> _jwtKeyService;
        private readonly IJwtTokenService<TClaim, TPrivateKey> _jwtTokenService;

        public AGenericJwtTokenManager(IJwtTokenService<TClaim, TPrivateKey> jwtTokenService, IJwtKeyService<TPrivateKey, TPublicKey> jwtKeyService)
        {
            _jwtTokenService = jwtTokenService;
            _jwtKeyService = jwtKeyService;
        }

        public virtual async Task<ApiResponse<string>> GenerateTokenAsync(TDto dto)
        {
            var privateKeyRes = _jwtKeyService.GetPrivateKey();
            if (privateKeyRes.Data is null)
            {
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Private key not found"
                };
            }
            var claimsDto = await FindClaimsAsync(dto);

            if (claimsDto is null)
            {
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Claims not found"
                };
            }

            var tokenResponse = await _jwtTokenService.GenerateTokenAsync(claimsDto, privateKeyRes.Data);
            return tokenResponse;
        }

        public abstract Task<TClaim?> FindClaimsAsync(TDto dto);
    }
}
