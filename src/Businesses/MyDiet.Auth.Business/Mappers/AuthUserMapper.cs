using BaseUtility;
using Microsoft.AspNetCore.Identity;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Enums;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Mappers
{
    internal class AuthUserMapper :
        IMapper<AuthUserDto, AuthUser>,
        IMapper<AuthUser, AuthUserDto>,
        IMapper<AuthUserDto, UserRegistrationResponse>,
        IMapper<UserRegistrationRequest, AuthUserDto>,
        IMapper<AuthUserDto, UserClaims>
    {
        public AuthUser Map(AuthUserDto input)
        {
            return new AuthUser
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = input.HashedPassword,
                CreatedAt = input.CreatedAt
            };
        }

        public AuthUserDto Map(AuthUser input)
        {
            return new AuthUserDto
            {
                Id = input.Id,
                Username = input.Username,
                Email = input.Email,
                HashedPassword = input.HashedPassword,
                Role = (UserRole)input.Role,
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
                Role = UserRole.User
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
                UserId = input.Id,
                Role = input.Role
            };
        }
    }
}
