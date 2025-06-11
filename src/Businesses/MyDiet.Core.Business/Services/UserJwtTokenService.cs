using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;

namespace MyDiet.Core.Business.Services
{
    internal class UserJwtTokenService : AGenericJwtTokenService<UserClaimDto>
    {
        public UserJwtTokenService(IJwtTokenGenerator<UserClaimDto> jwtTokenGenerator) : base(jwtTokenGenerator)
        {
        }
    }
}
