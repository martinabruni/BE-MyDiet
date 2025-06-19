using Azure.Security.KeyVault.Secrets;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Domain.Options;

namespace MyDiet.Identity.Infrastructure.Repositories
{
    public class KeyVaultSecretRepository : IJwtKeyRepository<KeyVaultSecret>
    {
        private readonly KeyVaultOption _vaultOption;
        private readonly SecretClient _secretClient;

        public KeyVaultSecretRepository(KeyVaultOption vaultOption, SecretClient secretClient)
        {
            _vaultOption = vaultOption;
            _secretClient = secretClient;
        }
        public async Task<KeyVaultSecret?> CreatePrivateKeyAsync(KeyVaultSecret value)
        {
            await _secretClient.SetSecretAsync(value);
            return value;
        }

        public async Task<KeyVaultSecret?> GetPrivateKeyAsync()
        {
            return await _secretClient.GetSecretAsync(_vaultOption.SecretName!);
        }
    }
}
