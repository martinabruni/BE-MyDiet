using BaseUtility;

namespace MyDiet.Auth.Domain.Dtos
{
    public class AuthUserDto : BaseDto<Guid>
    {
        public string? Username { get; set; } = string.Empty;

        public required string Email { get; set; }

        public required string HashedPassword { get; set; }
    }
}
