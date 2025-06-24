using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Mappers
{
    internal class AuthUserMapper : IMapper<AuthUserDto, User>, IMapper<User, AuthUserDto>
    {
        public User Map(AuthUserDto input)
        {
            return new User
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = input.HashedPassword,
            };
        }

        public AuthUserDto Map(User input)
        {
            return new AuthUserDto
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = input.HashedPassword,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt,
            };
        }
    }
}
