using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;

namespace MyDiet.Auth.Business.Managers
{
    internal class KeyPairManager : IKeyPairManager
    {
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;
        private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
        private readonly KeyPair _keyPair;
        private readonly KeyPairMessageOption _keyPairMessageOption;

        public KeyPairManager(IVaultService<KeyVaultSecret> keyPairService, IVaultService<JsonWebKeySetDto> publicKeyService, KeyPair keyPair, KeyPairMessageOption keyPairMessageOption)
        {
            _privateKeyService = keyPairService;
            _publicKeyService = publicKeyService;
            _keyPair = keyPair;
            _keyPairMessageOption = keyPairMessageOption;
        }
        public async Task<BusinessResponse<KeyPair>> RegenerateAsync()
        {
            var privateKeyRes = await _privateKeyService.CreateAsync();
            if (privateKeyRes.Data is null)
            {
                return BusinessResponse<KeyPair>.InternalServerError(_keyPairMessageOption.ErrorCreatingEntity);
            }
            var publicKeyRes = await _publicKeyService.CreateAsync();
            if (publicKeyRes.Data is null)
            {
                return BusinessResponse<KeyPair>.InternalServerError(_keyPairMessageOption.ErrorCreatingEntity);
            }

            _keyPair.PrivateKey = privateKeyRes.Data;
            _keyPair.PublicKey = publicKeyRes.Data;

            return BusinessResponse<KeyPair>.Created(_keyPair, _keyPairMessageOption.EntityCreatedSuccessfully);
        }
    }
}
