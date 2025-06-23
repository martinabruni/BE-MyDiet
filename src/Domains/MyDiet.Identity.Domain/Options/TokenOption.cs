namespace MyDiet.Identity.Domain.Options
{
    public class TokenOption
    {
        public required string Issuer { get; set; }
        public string Audience { get; set; } = string.Empty;
        public required int ExpiryMinutes { get; set; }
        public required string Algorithm { get; set; }
    }
}
