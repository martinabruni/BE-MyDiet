using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Business.Services
{
    internal abstract class AGenericJwtTokenService<TClaim, TKey> : IJwtTokenService<TClaim, TKey> where TKey : AsymmetricAlgorithm where TClaim : class
    {
        private readonly IJwtTokenGenerator<TClaim> _jwtTokenGenerator;
        private readonly IKeyProvider<TKey> _keyProvider;

        public AGenericJwtTokenService(IJwtTokenGenerator<TClaim> jwtTokenGenerator, IKeyProvider<TKey> keyProvider)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _keyProvider = keyProvider;
        }

        public async Task<string> GetPublicKey()
        {
            try
            {
                TKey rsa = await _keyProvider.GetPrivateKeyAsync();
                var publicKey = rsa.ExportSubjectPublicKeyInfo();
                return Convert.ToBase64String(publicKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Internal server error: {ex.Message}");
                return Task.FromResult<string>("").Result;
            }
        }
        public async Task<string> GenerateTokenAsync(TClaim claimDto)
        {
            try
            {
                return await _jwtTokenGenerator.GenerateTokenAsync(claimDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Internal server error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
