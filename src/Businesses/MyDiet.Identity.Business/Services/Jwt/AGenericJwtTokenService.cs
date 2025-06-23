using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Domain.Services;
using MyDiet.Shared.Domain.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MyDiet.Identity.Business.Services.Jwt
{
    internal abstract class AGenericJwtTokenService<TClaim, TPrivateKey>
    : IJwtTokenService<TClaim, TPrivateKey>
    where TClaim : class
    where TPrivateKey : AsymmetricSecurityKey
    {
        private readonly TokenOption _tokenOption;

        protected AGenericJwtTokenService(TokenOption tokenOption)
        {
            _tokenOption = tokenOption;
        }

        public abstract List<Claim> BuildClaims(TClaim claimDto);

        public virtual Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto, TPrivateKey privateKey)
        {
            try
            {
                var jwt = new JwtSecurityToken(
                    issuer: _tokenOption.Issuer,
                    audience: _tokenOption.Audience,
                    claims: BuildClaims(claimDto),
                    expires: DateTime.UtcNow.AddMinutes(_tokenOption.ExpiryMinutes),
                    signingCredentials: new SigningCredentials(privateKey, _tokenOption.Algorithm)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Task.FromResult(new ApiResponse<string>()
                {
                    Data = token,
                    StatusCode = HttpStatusCode.Created,
                    Message = "Token generated successfully."
                });
            }
            catch
            {
                return Task.FromResult(new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while generating the token"
                });
            }
        }
    }
}
