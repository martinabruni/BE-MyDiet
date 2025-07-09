using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyDiet.Auth.Business.Services
{
    internal class TokenService : ITokenService
    {
        private readonly TokenOption _tokenOption;
        private readonly IMapper<UserClaims, List<Claim>> _claimMapper;
        private readonly IMapper<KeyVaultSecret, RsaSecurityKey> _keyMapper;
        private readonly IMapper<JwtSecurityToken, TokenResponse> _tokenResponseMapper;
        private readonly ResponseMessage _responseMessageOption;

        public TokenService(TokenOption tokenOption, IMapper<UserClaims, List<Claim>> claimMapper, IMapper<KeyVaultSecret, RsaSecurityKey> keyMapper, IMapper<JwtSecurityToken, TokenResponse> tokenResponseMapper, ResponseMessage responseMessageOption)
        {
            _tokenOption = tokenOption;
            _claimMapper = claimMapper;
            _keyMapper = keyMapper;
            _tokenResponseMapper = tokenResponseMapper;
            _responseMessageOption = responseMessageOption;
        }

        public BusinessResponse<TokenResponse> GenerateToken(UserClaims claimDto, KeyVaultSecret privateKey)
        {
            try
            {
                var jwt = new JwtSecurityToken(
                    issuer: _tokenOption.Issuer,
                    audience: _tokenOption.Audience,
                    claims: _claimMapper.Map(claimDto),
                    expires: DateTime.UtcNow.AddMinutes(_tokenOption.ExpiryMinutes),
                    signingCredentials: new SigningCredentials(_keyMapper.Map(privateKey), _tokenOption.Algorithm)
                );
                return BusinessResponse<TokenResponse>.Created(
                    _tokenResponseMapper.Map(jwt), _responseMessageOption.EntityCreatedSuccessfully);
            }
            catch
            {
                return BusinessResponse<TokenResponse>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }
        }

        public Task<BusinessResponse<TokenResponse>> RevokeTokenAsync(string token)
        {
            //TODO: Implement token revocation logic
            return Task.FromResult(BusinessResponse<TokenResponse>.NotImplemented(_responseMessageOption.NotImplemented));
        }
    }
}
