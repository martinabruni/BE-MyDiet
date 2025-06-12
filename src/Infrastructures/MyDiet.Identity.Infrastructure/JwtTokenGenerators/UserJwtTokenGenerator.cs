using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Infrastructure.Abstractions;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.JwtTokenGenerators
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
                new Claim("userId", claimDto.UserId.ToString()),
            };
        }
    }
}
