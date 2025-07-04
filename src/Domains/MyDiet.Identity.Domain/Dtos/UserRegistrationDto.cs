namespace MyDiet.Identity.Domain.Dtos
{
    public class UserRegistrationDto
    {
        public required string Email { get; set; }
        public string Username { get; set; } = string.Empty;
        public required string Password { get; set; }
    }
}
