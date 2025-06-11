namespace MyDiet.Core.Security
{
    internal class JwtSettings
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public int ExpiryMinutes { get; init; }
        public bool UseKeyVault { get; init; }
        public string? VaultUri { get; init; }
        public string? KeyName { get; init; }
        public string? PrivateKeyPath { get; init; }
    }
}
