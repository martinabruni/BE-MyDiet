using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Abstractions;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Options;
using System.Security.Claims;

namespace MyDiet.Identity.Business.Services
{
    internal class UserJwtTokenService : AGenericJwtTokenService<UserClaimDto, RsaSecurityKey>
    {
        public UserJwtTokenService(TokenOption jwtSettings) : base(jwtSettings)
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
