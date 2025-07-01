namespace MyDiet.Auth.Domain.Dtos.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }
    }
}
