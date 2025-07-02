using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Managers
{
    internal class TokenManager : ITokenManager
    {
        private readonly IService<AuthUserDto, AuthUser, Guid> _authUserService;
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
        private readonly IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> _publicKeyMapper;
        private readonly ITokenService _tokenService;
        private readonly TokenOption _tokenOption;

        public TokenManager(IVaultService<KeyVaultSecret> privateKeyService, ITokenService tokenService, IService<AuthUserDto, AuthUser, Guid> authUserService, IVaultService<JsonWebKeySetDto> publicKeyService, TokenOption tokenOption, IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> publicKeyMapper)
        {
            _privateKeyService = privateKeyService;
            _tokenService = tokenService;
            _authUserService = authUserService;
            _publicKeyService = publicKeyService;
            _tokenOption = tokenOption;
            _publicKeyMapper = publicKeyMapper;
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

            return _tokenService.GenerateToken(claims, privateKeyRes.Data);
        }

        public async Task<BusinessResponse<TokenValidationParameters>> GetValidationParametersAsync()
        {
            await _publicKeyService.CreateAsync();
            var publicKeyRes = await _publicKeyService.GetAsync();
            if (publicKeyRes.Data is null)
            {
                return new BusinessResponse<TokenValidationParameters>()
                {
                    StatusCode = publicKeyRes.StatusCode,
                    Message = publicKeyRes.Message
                };
            }

            return new BusinessResponse<TokenValidationParameters>()
            {
                Data = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _tokenOption.Issuer,
                    IssuerSigningKey = _publicKeyMapper.Map(publicKeyRes.Data).FirstOrDefault(),
                },
                StatusCode = BusinessCode.Ok,
                Message = "Validation key retrieved successfully"
            };
        }

        public async Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token)
        {
            return await _tokenService.RevokeTokenAsync(token);
        }
    }
}
