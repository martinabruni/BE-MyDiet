using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.Abstractions
{
    internal abstract class AGenericJwtTokenGenerator<TClaim> : IJwtTokenGenerator<TClaim> where TClaim : class
    {
        public readonly JwtSettings _jwtSettings;
        //TODO: add generic TKey
        public readonly IKeyProvider<RSA> _keyProvider;

        protected AGenericJwtTokenGenerator(IKeyProvider<RSA> keyProvider, JwtSettings jwtSettings)
        {
            this._keyProvider = keyProvider;
            this._jwtSettings = jwtSettings;
        }

        public abstract List<Claim> BuildClaims(TClaim claimDto);

        public virtual async Task<string> GenerateTokenAsync(TClaim claimDto)
        {
            RSA rsa = await _keyProvider.GetPrivateKeyAsync();
            //TODO: replace key type and algorithm with generic TKey and SecurityAlgorithms
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
