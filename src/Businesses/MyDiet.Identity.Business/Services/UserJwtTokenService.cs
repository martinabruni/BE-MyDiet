using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Business.Services
{
    internal class UserJwtTokenService : AGenericJwtTokenService<UserClaimDto>
    {
        public UserJwtTokenService(IJwtTokenGenerator<UserClaimDto> jwtTokenGenerator, IKeyProvider keyProvider) : base(jwtTokenGenerator, keyProvider)
        {
        }
    }
}
