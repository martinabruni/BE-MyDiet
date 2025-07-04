using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Mappers
{
    internal class CoreUserMapper : IMapper<CoreUser, CoreUserDto>, IMapper<CoreUserDto, CoreUser>, IMapper<UserClaims, CoreUserDto>
    {
        public CoreUser Map(CoreUserDto input)
        {
            return new CoreUser
            {
                Id = input.Id,
                Username = input.Username,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public CoreUserDto Map(CoreUser input)
        {
            return new CoreUserDto
            {
                Id = input.Id,
                Username = input.Username,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public CoreUserDto Map(UserClaims input)
        {
            return new CoreUserDto
            {
                Id = input.UserId,
                Username = input.Username,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
