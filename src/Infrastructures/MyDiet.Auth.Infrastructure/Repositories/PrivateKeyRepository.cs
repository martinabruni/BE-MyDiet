using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Repositories;

namespace MyDiet.Auth.Infrastructure.Repositories
{
    internal class PrivateKeyRepository : IVaultRepository<KeyVaultSecret>
    {
        private readonly SecretClient _secretClient;
        private readonly VaultMessageOption _responseMessageOptions;

        public PrivateKeyRepository(SecretClient secretClient, VaultMessageOption responseMessageOptions)
        {
            _secretClient = secretClient;
            _responseMessageOptions = responseMessageOptions;
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> CreateSecretAsync(KeyVaultSecret secret)
        {
            if (secret is null)
            {
                return RepositoryResponse<KeyVaultSecret>.BadRequest(_responseMessageOptions.InvalidRequest);
            }
            try
            {
                var vaultSecret = await _secretClient.SetSecretAsync(secret);
                return RepositoryResponse<KeyVaultSecret>.Created(_responseMessageOptions.EntityCreatedSuccessfully, vaultSecret);
            }
            catch (ArgumentNullException)
            {
                return RepositoryResponse<KeyVaultSecret>.NotFound(_responseMessageOptions.EntityNotFound);
            }
            catch
            {
                return RepositoryResponse<KeyVaultSecret>.InternalServerError(_responseMessageOptions.ErrorCreatingEntity);
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> GetDeletedSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return  RepositoryResponse<KeyVaultSecret>.BadRequest(_responseMessageOptions.InvalidRequest);
            }

            try
            {
                var secret = await _secretClient.GetDeletedSecretAsync(secretName);
                return RepositoryResponse<KeyVaultSecret>.Ok(_responseMessageOptions.EntityRetrievedSuccessfully, secret);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return RepositoryResponse<KeyVaultSecret>.NotFound(_responseMessageOptions.EntityNotFound);
            }
            catch
            {
                return RepositoryResponse<KeyVaultSecret>.InternalServerError(_responseMessageOptions.ErrorRetrievingEntity);
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> GetSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return RepositoryResponse<KeyVaultSecret>.BadRequest(_responseMessageOptions.InvalidRequest);
            }

            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return RepositoryResponse<KeyVaultSecret>.Ok(_responseMessageOptions.EntityRetrievedSuccessfully, secret);
            }
            catch (CredentialUnavailableException ex)
            {
                return RepositoryResponse<KeyVaultSecret>.Unauthorize(_responseMessageOptions.InvalidCredentials);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return RepositoryResponse<KeyVaultSecret>.NotFound(_responseMessageOptions.EntityNotFound);
            }
            catch
            {
                return RepositoryResponse<KeyVaultSecret>.InternalServerError(_responseMessageOptions.ErrorRetrievingEntity);
            }
        }

        public async Task<RepositoryResponse<KeyVaultSecret>> PurgeSecretAsync(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                return RepositoryResponse<KeyVaultSecret>.BadRequest(_responseMessageOptions.InvalidRequest);
            }

            try
            {
                await _secretClient.PurgeDeletedSecretAsync(secretName);
                return RepositoryResponse<KeyVaultSecret>.Ok(_responseMessageOptions.EntityPurgedSuccessfully);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return RepositoryResponse<KeyVaultSecret>.NotFound(_responseMessageOptions.EntityNotFound);
            }
            catch
            {
                return RepositoryResponse<KeyVaultSecret>.InternalServerError(_responseMessageOptions.ErrorPurgingEntity);
            }
        }
    }
}
