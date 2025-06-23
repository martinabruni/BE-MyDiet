using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Mappers
{
    internal class UserMapper : IMapper<User, UserDto>, IMapper<UserDto, User>
    {
        public UserDto Map(User input)
        {
            return new UserDto
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email
            };
        }

        public User Map(UserDto input)
        {
            return new User
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = string.Empty,
            };
        }
    }
}
