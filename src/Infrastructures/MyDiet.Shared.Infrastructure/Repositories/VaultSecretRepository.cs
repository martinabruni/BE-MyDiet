using Azure;
using Azure.Security.KeyVault.Secrets;
using MyDiet.Identity.Domain.Repositories;

namespace MyDiet.Shared.Infrastructure.Repositories
{
    public class VaultSecretRepository : IVaultRepository<KeyVaultSecret, Response>
    {
        private readonly SecretClient _secretClient;

        public VaultSecretRepository(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<KeyVaultSecret?> CreateSecretAsync(KeyVaultSecret secret)
        {
            return await _secretClient.SetSecretAsync(secret);
        }

        public async Task<KeyVaultSecret?> GetDeletedSecretAsync(string secretName)
        {
            return await _secretClient.GetDeletedSecretAsync(secretName);
        }

        public async Task<KeyVaultSecret?> GetSecretAsync(string secretName)
        {
            return await _secretClient.GetSecretAsync(secretName);
        }

        public async Task<Response?> PurgeDeletedSecretAsync(string secretName)
        {
            return await _secretClient.PurgeDeletedSecretAsync(secretName);
        }
    }
}
