using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Repositories;
using MyDiet.Auth.Domain.Services;

namespace MyDiet.Auth.Business.Services
{
    internal class PublicKeyService : IVaultService<JsonWebKeySetDto>
    {
        private readonly IVaultRepository<KeyVaultSecret> _privateKeyRepository;
        private readonly IMapper<KeyVaultSecret, JsonWebKeySetDto> _secretToJwksMapper;
        private readonly KeyPair _keyPair;
        private readonly KeyOption _keyOption;
        private readonly KeyPairMessageOption _keyPairMessageOption;

        public PublicKeyService(IVaultRepository<KeyVaultSecret> vaultRepository, KeyPair keyPair, IMapper<KeyVaultSecret, JsonWebKeySetDto> secretToJwksMapper, KeyOption keyOption, KeyPairMessageOption keyPairMessageOption)
        {
            _privateKeyRepository = vaultRepository;
            _keyPair = keyPair;
            _secretToJwksMapper = secretToJwksMapper;
            _keyOption = keyOption;
            _keyPairMessageOption = keyPairMessageOption;
        }

        public async Task<BusinessResponse<JsonWebKeySetDto>> CreateAsync()
        {
            if (_keyPair.PublicKey is not null)
            {
                return BusinessResponse<JsonWebKeySetDto>.BadRequest(_keyPairMessageOption.KeyAlreadySet);
            }

            if (_keyPair.PrivateKey is not null)
            {
                _keyPair.PublicKey = _secretToJwksMapper.Map(_keyPair.PrivateKey);
                return BusinessResponse<JsonWebKeySetDto>.Created(_keyPairMessageOption.EntityCreatedSuccessfully, _keyPair.PublicKey);
            }

            var secretRes = await _privateKeyRepository.GetSecretAsync(_keyOption.PrivateKeyName);

            if (secretRes.Data is null)
            {
                return BusinessResponse<JsonWebKeySetDto>.NotFound(_keyPairMessageOption.EntityNotFound);
            }
            _keyPair.PublicKey = _secretToJwksMapper.Map(secretRes.Data);

            return BusinessResponse<JsonWebKeySetDto>.Created(_keyPairMessageOption.EntityCreatedSuccessfully, _keyPair.PublicKey);
        }

        public Task<BusinessResponse<JsonWebKeySetDto>> GetDeletedAsync()
        {
            return Task.FromResult(BusinessResponse<JsonWebKeySetDto>.NotImplemented(_keyPairMessageOption.NotImplemented));
        }

        public Task<BusinessResponse<JsonWebKeySetDto>> PurgeDeletedAsync()
        {
            return Task.FromResult(BusinessResponse<JsonWebKeySetDto>.NotImplemented(_keyPairMessageOption.NotImplemented));
        }

        public Task<BusinessResponse<JsonWebKeySetDto>> GetAsync()
        {
            if (_keyPair.PublicKey is null)
            {
                return Task.FromResult(BusinessResponse<JsonWebKeySetDto>.NotFound(_keyPairMessageOption.EntityNotFound));
            }
            return Task.FromResult(BusinessResponse<JsonWebKeySetDto>.Ok(_keyPairMessageOption.EntityRetrievedSuccessfully));
        }
    }
}
