using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Repositories;

namespace MyDiet.Auth.Infrastructure.Repositories
{
    internal class PrivateKeyRepository : IVaultRepository<KeyVaultSecret>
    {
        private readonly SecretClient _secretClient;

        public PrivateKeyRepository(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> CreateSecretAsync(KeyVaultSecret secret)
        {
            if (secret is null)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Secret cannot be null."
                };
            }
            try
            {
                var vaultSecret = await _secretClient.SetSecretAsync(secret);
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.Created,
                    Data = vaultSecret,
                    Message = "Secret created successfully."
                };
            }
            catch (ArgumentNullException)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.NotFound,
                    Message = "Failed to create secret in Key Vault."
                };
            }
            catch
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error creating secret"
                };
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> GetDeletedSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Secret name cannot be null or empty."
                };
            }

            try
            {
                var secret = await _secretClient.GetDeletedSecretAsync(secretName);
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = secret.Value,
                    Message = "Deleted secret retrieved successfully."
                };
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.NotFound,
                    Message = "Deleted secret not found."
                };
            }
            catch
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error retrieving deleted secret."
                };
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> GetSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Secret name cannot be null or empty."
                };
            }

            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = secret.Value,
                    Message = "Secret retrieved successfully."
                };
            }
            catch (CredentialUnavailableException ex)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.Unauthorized,
                    Message = ex.Message,
                };
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.NotFound,
                    Message = "Secret not found."
                };
            }
            catch
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error retrieving secret."
                };
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> PurgeSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Secret name cannot be null or empty."
                };
            }

            try
            {
                await _secretClient.PurgeDeletedSecretAsync(secretName);
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.Ok,
                    Message = "Secret purged successfully."
                };
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.NotFound,
                    Message = "Secret not found for purging."
                };
            }
            catch
            {
                return new RepositoryResponse<KeyVaultSecret>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error purging secret."
                };
            }
        }
    }
}
