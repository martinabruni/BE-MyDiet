using MyDiet.Identity.Domain.Dtos;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyDiet.Identity.Business.Mappers
{
    internal class IdentityUserMapper : IMapper<UserRegistrationDto, IdentityUserDto>, IMapper<User, IdentityUserDto>, IMapper<IdentityUserDto, User>
    {
        public IdentityUserDto Map(UserRegistrationDto input)
        {
            return new IdentityUserDto
            {
                Id = Guid.NewGuid(),
                Email = input.Email,
                Username = input.Username,
                HashedPassword = HashPassword(input.Password),
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public User Map(IdentityUserDto input)
        {
            return new User
            {
                Id = input.Id,
                Email = input.Email,
                Username = input.Username,
                HashedPassword = input.HashedPassword
            };
        }

        public IdentityUserDto Map(User input)
        {
            return new IdentityUserDto
            {
                Id = input.Id,
                Email = input.Email,
                Username = input.Username,
                HashedPassword = input.HashedPassword,
            };
        }
    }
}
