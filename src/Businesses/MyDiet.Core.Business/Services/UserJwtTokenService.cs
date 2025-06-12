using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Business.Services
{
    internal class UserJwtTokenService : AGenericJwtTokenService<UserClaimDto, RSA>
    {
        public UserJwtTokenService(IJwtTokenGenerator<UserClaimDto> jwtTokenGenerator, IKeyProvider<RSA> keyProvider) : base(jwtTokenGenerator, keyProvider)
        {
        }
    }
}
