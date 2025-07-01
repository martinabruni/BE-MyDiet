namespace MyDiet.Auth.Domain.Dtos.Requests
{
    public class UserRegistrationRequest
    {
        public string? Username { get; set; } = string.Empty;

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
