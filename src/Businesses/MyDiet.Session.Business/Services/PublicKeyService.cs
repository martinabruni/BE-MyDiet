using Azure.Security.KeyVault.Secrets;
using MyDiet.Session.Domain.Mappers;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Options;
using MyDiet.Session.Domain.Repositories;
using MyDiet.Session.Domain.Responses;
using MyDiet.Session.Domain.Services;

namespace MyDiet.Session.Business.Services
{
    internal class PublicKeyService : IVaultService<JsonWebKeySetDto>
    {
        private readonly IVaultRepository<KeyVaultSecret> _privateKeyRepository;
        private readonly IMapper<KeyVaultSecret, JsonWebKeySetDto> _secretToJwksMapper;
        private readonly KeyPair _keyPair;
        private readonly KeyOption _keyOption;

        public PublicKeyService(IVaultRepository<KeyVaultSecret> vaultRepository, KeyPair keyPair, IMapper<KeyVaultSecret, JsonWebKeySetDto> secretToJwksMapper, KeyOption keyOption)
        {
            _privateKeyRepository = vaultRepository;
            _keyPair = keyPair;
            _secretToJwksMapper = secretToJwksMapper;
            _keyOption = keyOption;
        }

        public async Task<BusinessResponse<JsonWebKeySetDto>> CreateAsync()
        {
            if (_keyPair.PublicKey is not null)
            {
                return new BusinessResponse<JsonWebKeySetDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Public key is already set.",
                };
            }

            if(_keyPair.PrivateKey is not null)
            {
                _keyPair.PublicKey = _secretToJwksMapper.Map(_keyPair.PrivateKey);
                return new BusinessResponse<JsonWebKeySetDto>
                {
                    StatusCode = BusinessCode.Created,
                    Message = "Public key created successfully from existing private key.",
                    Data = _keyPair.PublicKey
                };
            }

            var secret = await _privateKeyRepository.GetSecretAsync(_keyOption.PrivateKeyName);

            if (secret.Data is null)
            {
                return new BusinessResponse<JsonWebKeySetDto>
                {
                    StatusCode = BusinessCode.NotFound,
                    Message = "Private key not found.",
                };
            }
            _keyPair.PublicKey = _secretToJwksMapper.Map(secret.Data);

            return new BusinessResponse<JsonWebKeySetDto>
            {
                StatusCode = BusinessCode.Created,
                Message = "Public key created successfully.",
                Data = _keyPair.PublicKey
            };
        }

        public async Task<BusinessResponse<JsonWebKeySetDto>> GetDeletedAsync()
        {
            return new BusinessResponse<JsonWebKeySetDto>
            {
                StatusCode = BusinessCode.NotImplemented,
                Message = "This method is not implemented for PublicKeyService."
            };
        }

        public async Task<BusinessResponse<JsonWebKeySetDto>> ExistsAsync()
        {
            if (_keyPair.PublicKey is null)
            {
                return new BusinessResponse<JsonWebKeySetDto>
                {
                    StatusCode = BusinessCode.NotFound,
                    Message = "Public key does not exist. Please create it first."
                };
            }
            return new BusinessResponse<JsonWebKeySetDto>
            {
                StatusCode = BusinessCode.Ok,
                Message = "Public key exists.",
                Data = _keyPair.PublicKey
            };
        }

        public async Task<BusinessResponse<JsonWebKeySetDto>> PurgeDeletedAsync()
        {
            return new BusinessResponse<JsonWebKeySetDto>
            {
                StatusCode = BusinessCode.NotImplemented,
                Message = "This method is not implemented for PublicKeyService."
            };
        }
    }
}
