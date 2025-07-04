namespace MyDiet.Shared.Domain.Dtos
{
    public class UserDto : BaseDto<Guid>
    {
        public required string Username { get; set; }

        public required string Email { get; set; }
    }
}
