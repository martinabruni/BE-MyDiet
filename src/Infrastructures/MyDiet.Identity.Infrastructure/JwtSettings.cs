namespace MyDiet.Identity.Infrastructure
{
    internal class JwtSettings
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiryMinutes { get; set; }
        public bool UseKeyVault { get; set; }
        public string? VaultUri { get; set; }
        public string? KeyName { get; set; }
        public string? PrivateKeyPath { get; set; }
    }
}
