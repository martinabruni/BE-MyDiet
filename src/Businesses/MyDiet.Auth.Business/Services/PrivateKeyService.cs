using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Repositories;
using MyDiet.Auth.Domain.Services;
using System.Security.Cryptography;

namespace MyDiet.Auth.Business.Services
{
    internal class PrivateKeyService : IVaultService<KeyVaultSecret>
    {
        private readonly IVaultRepository<KeyVaultSecret> _privateKeyRepository;
        private readonly IMapper<RSA, KeyVaultSecret> _rsaToSecretMapper;
        private readonly KeyPair _keyPair;
        private readonly KeyOption _keyOption;
        private readonly KeyPairMessageOption _keyPairMessageOption;

        public PrivateKeyService(IVaultRepository<KeyVaultSecret> vaultRepository, KeyPair keyPair, IMapper<RSA, KeyVaultSecret> rsaToSecretMapper, KeyOption keyOption, KeyPairMessageOption keyPairMessageOption)
        {
            _privateKeyRepository = vaultRepository;
            _keyPair = keyPair;
            _rsaToSecretMapper = rsaToSecretMapper;
            _keyOption = keyOption;
            _keyPairMessageOption = keyPairMessageOption;
        }

        public async Task<BusinessResponse<KeyVaultSecret>> CreateAsync()
        {
            if (_keyPair.PrivateKey is not null)
            {
                return BusinessResponse<KeyVaultSecret>.BadRequest(_keyPairMessageOption.KeyAlreadySet);
            }

            var existingSecret = await _privateKeyRepository.GetSecretAsync(_keyOption.PrivateKeyName);

            if (existingSecret.Data is not null)
            {
                _keyPair.PrivateKey = existingSecret.Data;
                return BusinessResponse<KeyVaultSecret>.Ok(_keyPairMessageOption.EntityRetrievedSuccessfully, _keyPair.PrivateKey);
            }

            var deletedSecret = await _privateKeyRepository.GetDeletedSecretAsync(_keyOption.PrivateKeyName);

            if (deletedSecret.Data is not null)
            {
                _privateKeyRepository.PurgeSecretAsync(_keyOption.PrivateKeyName).GetAwaiter().GetResult();
            }

            var secret = _rsaToSecretMapper.Map(RSA.Create(_keyOption.KeySize));
            var vaultResponse = await _privateKeyRepository.CreateSecretAsync(secret);

            if (vaultResponse.StatusCode != RepositoryCode.Created)
            {
                return vaultResponse.ToBusinessResponse();
            }
            _keyPair.PrivateKey = secret;

            return BusinessResponse<KeyVaultSecret>.Created(_keyPairMessageOption.EntityCreatedSuccessfully, _keyPair.PrivateKey);
        }

        public async Task<BusinessResponse<KeyVaultSecret>> GetDeletedAsync()
        {
            var res = await _privateKeyRepository.GetDeletedSecretAsync(_keyOption.PrivateKeyName);
            return res.ToBusinessResponse();
        }

        public async Task<BusinessResponse<KeyVaultSecret>> GetAsync()
        {
            if (_keyPair.PrivateKey is null)
            {
                var privateKeyResponse = await _privateKeyRepository.GetSecretAsync(_keyOption.PrivateKeyName);
                if (privateKeyResponse.Data is null)
                {
                    return BusinessResponse<KeyVaultSecret>.NotFound(_keyPairMessageOption.EntityNotFound);
                }
                _keyPair.PrivateKey = privateKeyResponse.Data;
            }
            return BusinessResponse<KeyVaultSecret>.Ok(_keyPairMessageOption.EntityRetrievedSuccessfully, _keyPair.PrivateKey);
        }

        public async Task<BusinessResponse<KeyVaultSecret>> PurgeDeletedAsync()
        {
            var res = await _privateKeyRepository.PurgeSecretAsync(_keyOption.PrivateKeyName);
            return res.ToBusinessResponse();
        }
    }
}
