using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Domain.Options;
using MyDiet.Shared.Domain.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MyDiet.Identity.Business.Abstractions
{
    internal abstract class AGenericJwtTokenService<TClaim> : IJwtTokenService<TClaim> where TClaim : class
    {
        private readonly TokenOption _tokenOption;

        protected AGenericJwtTokenService(TokenOption tokenOption)
        {
            _tokenOption = tokenOption;
        }

        public abstract List<Claim> BuildClaims(TClaim claimDto);

        public virtual Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto)
        {
            var jwt = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                audience: _tokenOption.Audience,
                claims: BuildClaims(claimDto),
                expires: DateTime.UtcNow.AddMinutes(_tokenOption.ExpiryMinutes),
                signingCredentials: _tokenOption.SigningCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Task.FromResult(new ApiResponse<string>()
            {
                Data = token,
                StatusCode = HttpStatusCode.OK,
                Message = "Token generated successfully."
            });
        }
    }
}
