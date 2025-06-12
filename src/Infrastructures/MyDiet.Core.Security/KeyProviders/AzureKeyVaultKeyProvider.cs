using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Security.KeyProviders
{
    internal class AzureKeyVaultKeyProvider : IKeyProvider<RSA>
    {
        private readonly JwtSettings _settings;

        public AzureKeyVaultKeyProvider(JwtSettings settings)
        {
            _settings = settings;
        }

        public async Task<RSA> GetKeyAsync()
        {
            var credential = new ManagedIdentityCredential();
            var client = new SecretClient(new Uri(_settings.VaultUri!), credential);
            KeyVaultSecret secret = await client.GetSecretAsync(_settings.KeyName!);
            var rsa = RSA.Create();
            rsa.ImportFromPem(secret.Value.ToCharArray());
            return rsa;
        }
    }
}
