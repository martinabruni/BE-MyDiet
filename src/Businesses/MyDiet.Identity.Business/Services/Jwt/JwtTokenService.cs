using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Options;
using System.Security.Claims;

namespace MyDiet.Identity.Business.Services.Jwt
{
    internal class JwtTokenService : AGenericJwtTokenService<UserClaims, RsaSecurityKey>
    {
        public JwtTokenService(TokenOption jwtSettings) : base(jwtSettings)
        {
        }

        public override List<Claim> BuildClaims(UserClaims claimDto)
        {
            var userId = claimDto.UserId;

            return
            [
                new Claim(nameof(userId).LowercaseFirst(), userId.ToString())
            ];
        }
    }
}
