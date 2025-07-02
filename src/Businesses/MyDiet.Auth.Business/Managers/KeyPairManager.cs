using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Services;

namespace MyDiet.Auth.Business.Managers
{
    internal class KeyPairManager : IKeyPairManager
    {
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
        private readonly IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> _publicKeyMapper;
        private readonly KeyPair _keyPair;

        public KeyPairManager(IVaultService<KeyVaultSecret> keyPairService, IVaultService<JsonWebKeySetDto> publicKeyService, KeyPair keyPair, IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> publicKeyMapper)
        {
            _privateKeyService = keyPairService;
            _publicKeyService = publicKeyService;
            _keyPair = keyPair;
            _publicKeyMapper = publicKeyMapper;
        }
        public async Task<BusinessResponse<KeyPair>> RegenerateAsync()
        {
            var privateKeyRes = await _privateKeyService.CreateAsync();
            if (privateKeyRes.Data is null)
            {
                return new BusinessResponse<KeyPair>
                {
                    StatusCode = privateKeyRes.StatusCode,
                    Message = privateKeyRes.Message,
                };
            }
            var publicKeyRes = await _publicKeyService.CreateAsync();
            if (publicKeyRes.Data is null)
            {
                return new BusinessResponse<KeyPair>
                {
                    StatusCode = publicKeyRes.StatusCode,
                    Message = publicKeyRes.Message,
                };
            }

            _keyPair.PrivateKey = privateKeyRes.Data;
            _keyPair.PublicKey = publicKeyRes.Data;

            return new BusinessResponse<KeyPair>
            {
                StatusCode = BusinessCode.Created,
                Message = "Key pair regenerated successfully."
            };
        }

        public async Task<BusinessResponse<IEnumerable<RsaSecurityKey>>> GetSigningKeyAsync()
        {
            var privateKeyRes = await _privateKeyService.GetAsync();
            if (privateKeyRes.Data is null)
            {
                return new BusinessResponse<IEnumerable<RsaSecurityKey>>
                {
                    StatusCode = privateKeyRes.StatusCode,
                    Message = privateKeyRes.Message,
                };
            }

            var publicKeyRes = await _publicKeyService.GetAsync();

            if (publicKeyRes.Data is null)
            {
                return new BusinessResponse<IEnumerable<RsaSecurityKey>>
                {
                    StatusCode = publicKeyRes.StatusCode,
                    Message = publicKeyRes.Message,
                };
            }

            var rsaSecurityKeys = _publicKeyMapper.Map(publicKeyRes.Data);

            return new BusinessResponse<IEnumerable<RsaSecurityKey>>
            {
                StatusCode = BusinessCode.Ok,
                Data = rsaSecurityKeys
            };
        }
    }
}
