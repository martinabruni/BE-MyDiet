using Azure.Security.KeyVault.Secrets;

namespace MyDiet.Identity.Domain.Dtos.Jwt
{
    public class KeyPairDto
    {
        public KeyVaultSecret? PrivateKey { get; set; } = default;
        public JsonWebKeySetDto? PublicKey { get; set; } = default;
    }
}
