using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Business.Services
{
    internal class UserJwtTokenService : AGenericJwtTokenService<UserClaimDto, RSA>
    {
        public UserJwtTokenService(IJwtTokenGenerator<UserClaimDto> jwtTokenGenerator, IKeyProvider<RSA> keyProvider) : base(jwtTokenGenerator, keyProvider)
        {
        }
    }
}
