using MyDiet.Shared.Domain.Dtos;

namespace MyDiet.Identity.Domain.Dtos
{
    public class IdentityUserDto : BaseDto<Guid>
    {
        public required string Email { get; set; }
        public string Username { get; set; } = string.Empty;
        public required string HashedPassword { get; set; }
    }
}
