using Azure.Identity;
using Azure.Security.KeyVault.Keys;
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
            var client = new KeyClient(new Uri(_settings.VaultUri!), credential);
            KeyVaultKey key = await client.GetKeyAsync(_settings.KeyName!);
            var jwk = key.Key;
            var rsa = RSA.Create();
            var rsaParams = new RSAParameters
            {
                Modulus = jwk.N!,
                Exponent = jwk.E!,
                D = jwk.D!,
                P = jwk.P!,
                Q = jwk.Q!,
                DP = jwk.DP!,
                DQ = jwk.DQ!,
                InverseQ = jwk.QI!
            };
            rsa.ImportParameters(rsaParams);
            return rsa;
            //return new CryptographyClient(key.Id, credential);
        }

        public async Task<RSA> GetSecretAsync()
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
