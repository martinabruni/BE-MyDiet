using Microsoft.IdentityModel.Tokens;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyDiet.Core.Security.Abstractions
{
    internal abstract class AGenericJwtTokenGenerator<TDto> : IJwtTokenGenerator<TDto>
    {
        public readonly JwtSettings _jwtSettings;
        public readonly IKeyProvider<RSA> _keyProvider;

        protected AGenericJwtTokenGenerator(IKeyProvider<RSA> keyProvider, JwtSettings jwtSettings)
        {
            this._keyProvider = keyProvider;
            this._jwtSettings = jwtSettings;
        }

        public abstract List<Claim> BuildClaims(TDto claimDto);

        public virtual async Task<string> GenerateTokenAsync(TDto claimDto)
        {
            RSA rsa = await _keyProvider.GetPrivateKeyAsync();
            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: BuildClaims(claimDto),
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
