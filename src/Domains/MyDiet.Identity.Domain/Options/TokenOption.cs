using Microsoft.IdentityModel.Tokens;

namespace MyDiet.Identity.Domain.Options
{
    public class TokenOption
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiryMinutes { get; set; }
        public SigningCredentials SigningCredentials { get; set; } = default!;
    }
}
