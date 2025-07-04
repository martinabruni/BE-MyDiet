using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Services;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Identity.Business.Managers.Jwt
{
    internal class UserJwtTokenManager : AGenericJwtTokenManager<UserRegistrationDto, UserClaims, RsaSecurityKey, JsonWebKeySetDto>
    {
        private readonly IService<UserDto, User, Guid> userService;

        public UserJwtTokenManager(IJwtTokenService<UserClaims, RsaSecurityKey> jwtTokenService, IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> jwtKeyService, IService<UserDto, User, Guid> userService) : base(jwtTokenService, jwtKeyService)
        {
            this.userService = userService;
        }

        public override async Task<UserClaims?> FindClaimsAsync(UserRegistrationDto dto)
        {
            var userRes = await userService.GetAsync(entity => entity.Email == dto.Email);

            if (userRes.Data is null)
                return null;

            var users = userRes.Data.ToList();
            if (users.Count == 0)
            {
                return null;
            }

            var tokenClaim = new UserClaims
            {
                UserId = users[0].Id,
            };
            return tokenClaim;
        }
    }
}
