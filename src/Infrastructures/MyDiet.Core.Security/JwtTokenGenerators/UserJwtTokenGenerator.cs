using Microsoft.IdentityModel.Tokens;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using MyDiet.Core.Security.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyDiet.Core.Security.JwtTokenGenerators
{
    internal class UserJwtTokenGenerator : AGenericJwtTokenGenerator<UserClaimDto>
    {
        public UserJwtTokenGenerator(IKeyProvider<RSA> keyProvider, JwtSettings jwtSettings) : base(keyProvider, jwtSettings)
        {
        }

        public override async Task<string> GenerateTokenAsync(UserClaimDto claimDto)
        {
            RSA rsa = await _keyProvider.GetKeyAsync();
            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, claimDto.UserId.ToString()),
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
