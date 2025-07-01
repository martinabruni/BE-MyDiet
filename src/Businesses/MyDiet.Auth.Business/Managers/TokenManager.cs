using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Services;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Managers
{
    internal class TokenManager : ITokenManager
    {
        private readonly IService<AuthUserDto, User, Guid> _authUserService;
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly ITokenService _tokenService;

        public TokenManager(IVaultService<KeyVaultSecret> privateKeyService, ITokenService tokenService, IService<AuthUserDto, User, Guid> authUserService)
        {
            _privateKeyService = privateKeyService;
            _tokenService = tokenService;
            _authUserService = authUserService;
        }

        public virtual async Task<BusinessResponse<TokenResponse>> GenerateTokenAsync(UserClaims claims)
        {
            if (claims is null)
            {
                return new BusinessResponse<TokenResponse>()
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Claims cannot be null"
                };
            }

            var userRes = await _authUserService.GetByIdAsync(claims.UserId);

            if (userRes.Data is null)
            {
                return new BusinessResponse<TokenResponse>()
                {
                    StatusCode = userRes.StatusCode,
                    Message = userRes.Message
                };
            }

            var privateKeyRes = await _privateKeyService.GetAsync();

            if (privateKeyRes.Data is null)
            {
                return new BusinessResponse<TokenResponse>()
                {
                    StatusCode = privateKeyRes.StatusCode,
                    Message = privateKeyRes.Message
                };
            }

            return await _tokenService.GenerateTokenAsync(claims, privateKeyRes.Data);
        }

        public async Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token)
        {
            return await _tokenService.RevokeTokenAsync(token);
        }
    }
}
