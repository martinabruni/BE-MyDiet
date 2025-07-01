namespace MyDiet.Auth.Domain.Dtos.Responses
{
    public class UserRegistrationResponse
    {
        public required Guid Id { get; set; }

        public string? Username { get; set; } = string.Empty;
        
        public required string Email { get; set; }
    }
}
