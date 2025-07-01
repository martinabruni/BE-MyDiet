using Azure.Security.KeyVault.Secrets;

namespace MyDiet.Auth.Domain.Models
{
    public class KeyPair
    {
        public required KeyVaultSecret PrivateKey { get; set; }
        public required JsonWebKeySetDto PublicKey { get; set; }
    }
}
