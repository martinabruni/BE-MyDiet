using BaseUtility;
using Microsoft.AspNetCore.Identity;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Mappers
{
    internal class AuthUserMapper :
        IMapper<AuthUserDto, User>,
        IMapper<User, AuthUserDto>,
        IMapper<AuthUserDto, UserRegistrationResponse>,
        IMapper<UserRegistrationRequest, AuthUserDto>
    {
        public User Map(AuthUserDto input)
        {
            return new User
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = input.HashedPassword,
                CreatedAt = input.CreatedAt
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

        public AuthUserDto Map(UserRegistrationRequest input)
        {
            return new AuthUserDto
            {
                Id = Guid.NewGuid(),
                Username = input.Username,
                Email = input.Email,
                HashedPassword = new PasswordHasher<object>().HashPassword(input, input.Password),
            };
        }

        UserRegistrationResponse IMapper<AuthUserDto, UserRegistrationResponse>.Map(AuthUserDto input)
        {
            return new UserRegistrationResponse
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
            };
        }
    }
}
