using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MyDiet.Identity.Infrastructure.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.KeyProviders
{
    internal class AzureKeyVaultKeyProvider : IPrivateKeyProvider<RSA>
    {
        private readonly JwtSettings _settings;
        private readonly RSA _rsa = RSA.Create(2048);

        public AzureKeyVaultKeyProvider(JwtSettings settings)
        {
            _settings = settings;
        }

        public async Task<RSA> GetPrivateKeyAsync()
        {
            var credential = new ManagedIdentityCredential();
            var client = new SecretClient(new Uri(_settings.VaultUri!), credential);
            KeyVaultSecret secret = await client.GetSecretAsync(_settings.KeyName!);
            _rsa.ImportFromPem(secret.Value.ToCharArray());
            return _rsa;
        }

        public Task<string> GetPublicKeyAsync()
        {
            var publicKeyPem = _rsa.ExportSubjectPublicKeyInfo();
            return Task.FromResult(PemEncoding.WriteString("PUBLIC KEY", publicKeyPem));
        }
    }
}
