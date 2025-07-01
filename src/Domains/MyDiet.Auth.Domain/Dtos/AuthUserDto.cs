using BaseUtility;
using MyDiet.Auth.Domain.Enums;

namespace MyDiet.Auth.Domain.Dtos
{
    public class AuthUserDto : BaseDto<Guid>
    {
        public string? Username { get; set; } = string.Empty;

        public required string Email { get; set; }

        public required string HashedPassword { get; set; }

        public required UserRole Role { get; set; } = 0;
    }
}
