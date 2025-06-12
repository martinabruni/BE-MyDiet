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

        public override List<Claim> BuildClaims(UserClaimDto claimDto)
        {
            return new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, claimDto.UserId.ToString()),
            };
        }
    }
}
