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
        private readonly IService<AuthUserDto, AuthUser, Guid> _authUserService;
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly ITokenService _tokenService;
        private readonly ResponseMessage _responseMessageOption;

        public TokenManager(IVaultService<KeyVaultSecret> privateKeyService, ITokenService tokenService, IService<AuthUserDto, AuthUser, Guid> authUserService, ResponseMessage responseMessageOption)
        {
            _privateKeyService = privateKeyService;
            _tokenService = tokenService;
            _authUserService = authUserService;
            _responseMessageOption = responseMessageOption;
        }

        public async Task<BusinessResponse<TokenResponse>> GenerateTokenAsync(UserClaims claims)
        {
            if (claims is null)
            {
                return BusinessResponse<TokenResponse>.BadRequest(_responseMessageOption.InvalidRequest);
            }

            var userRes = await _authUserService.GetByIdAsync(claims.UserId);

            if (userRes.Data is null)
            {
                return BusinessResponse<TokenResponse>.NotFound(_responseMessageOption.EntityNotFound);
            }

            var privateKeyRes = await _privateKeyService.GetAsync();

            if (privateKeyRes.Data is null)
            {
                return BusinessResponse<TokenResponse>.NotFound(_responseMessageOption.EntityNotFound);
            }

            return _tokenService.GenerateToken(claims, privateKeyRes.Data);
        }

        public async Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token)
        {
            return await _tokenService.RevokeTokenAsync(token);
        }
    }
}
