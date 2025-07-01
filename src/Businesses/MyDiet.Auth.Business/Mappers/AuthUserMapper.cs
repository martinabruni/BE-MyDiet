using BaseUtility;
using Microsoft.AspNetCore.Identity;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Mappers
{
    internal class AuthUserMapper :
        IMapper<AuthUserDto, User>,
        IMapper<User, AuthUserDto>,
        IMapper<AuthUserDto, UserRegistrationResponse>,
        IMapper<UserRegistrationRequest, AuthUserDto>,
        IMapper<AuthUserDto, UserClaims>
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
            var id = Guid.NewGuid();
            return new AuthUserDto
            {
                Id = id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = new PasswordHasher<object>().HashPassword(id, input.Password),
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

        UserClaims IMapper<AuthUserDto, UserClaims>.Map(AuthUserDto input)
        {
            return new UserClaims
            {
                UserId = input.Id
            };
        }
    }
}
