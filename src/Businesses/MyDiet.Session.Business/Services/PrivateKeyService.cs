using Azure.Security.KeyVault.Secrets;
using MyDiet.Session.Domain.Mappers;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Options;
using MyDiet.Session.Domain.Repositories;
using MyDiet.Session.Domain.Responses;
using MyDiet.Session.Domain.Services;
using System.Security.Cryptography;

namespace MyDiet.Session.Business.Services
{
    internal class PrivateKeyService : IVaultService<KeyVaultSecret>
    {
        private readonly IVaultRepository<KeyVaultSecret> _privateKeyRepository;
        private readonly IMapper<KeyVaultSecret, JsonWebKeySetDto> _secretToJwksMapper;
        private readonly IMapper<RSA, KeyVaultSecret> _rsaToSecretMapper;
        private readonly KeyPair _keyPair;
        private readonly KeyOption _keyOption;

        public PrivateKeyService(IVaultRepository<KeyVaultSecret> vaultRepository, KeyPair keyPair, IMapper<KeyVaultSecret, JsonWebKeySetDto> secretToJwksMapper, IMapper<RSA, KeyVaultSecret> rsaToSecretMapper, KeyOption keyOption)
        {
            _privateKeyRepository = vaultRepository;
            _keyPair = keyPair;
            _secretToJwksMapper = secretToJwksMapper;
            _rsaToSecretMapper = rsaToSecretMapper;
            _keyOption = keyOption;
        }

        public async Task<BusinessResponse<KeyVaultSecret>> CreateAsync()
        {
            if (_keyPair.PrivateKey is not null)
            {
                return new BusinessResponse<KeyVaultSecret>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Private key is already set.",
                };
            }
            var secret = _rsaToSecretMapper.Map(RSA.Create(_keyOption.KeySize));
            var vaultResponse = await _privateKeyRepository.CreateSecretAsync(secret);
            if (vaultResponse.StatusCode != RepositoryCode.Created)
            {
                return vaultResponse.ToBusinessResponse();
            }
            _keyPair.PrivateKey = secret;

            return new BusinessResponse<KeyVaultSecret>
            {
                StatusCode = BusinessCode.Created,
                Message = "Secret created successfully.",
            };
        }

        public async Task<BusinessResponse<KeyVaultSecret>> GetDeletedAsync()
        {
            var res = await _privateKeyRepository.GetDeletedSecretAsync(_keyOption.PrivateKeyName);
            return res.ToBusinessResponse();
        }

        public async Task<BusinessResponse<KeyVaultSecret>> ExistsAsync()
        {
            if (_keyPair.PrivateKey is null)
            {
                var privateKeyResponse = await _privateKeyRepository.GetSecretAsync(_keyOption.PrivateKeyName);
                return new BusinessResponse<KeyVaultSecret>
                {
                    StatusCode = (BusinessCode)privateKeyResponse.StatusCode,
                    Message = privateKeyResponse.Message,
                };
            }
            return new BusinessResponse<KeyVaultSecret>
            {
                StatusCode = BusinessCode.Success,
                Message = "Private key exists."
            };
        }

        public async Task<BusinessResponse<KeyVaultSecret>> PurgeDeletedAsync()
        {
            var res = await _privateKeyRepository.PurgeSecretAsync(_keyOption.PrivateKeyName);
            return res.ToBusinessResponse();
        }
    }
}
