using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Abstractions;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Business.Managers
{
    internal class UserJwtTokenManager : AGenericJwtTokenManager<UserClaimDto, RsaSecurityKey, JsonWebKeySetDto>, IJwtTokenManager<UserClaimDto, RsaSecurityKey, JsonWebKeySetDto>
    {
        public UserJwtTokenManager(IJwtTokenService<UserClaimDto, RsaSecurityKey> jwtTokenService, IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> jwtKeyService) : base(jwtTokenService, jwtKeyService)
        {
        }
    }
}
