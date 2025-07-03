using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Services;

namespace MyDiet.Auth.Business.Managers
{
    internal class KeyPairManager : IKeyPairManager
    {
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
        private readonly KeyPair _keyPair;

        public KeyPairManager(IVaultService<KeyVaultSecret> keyPairService, IVaultService<JsonWebKeySetDto> publicKeyService, KeyPair keyPair)
        {
            _privateKeyService = keyPairService;
            _publicKeyService = publicKeyService;
            _keyPair = keyPair;
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
    }
}
