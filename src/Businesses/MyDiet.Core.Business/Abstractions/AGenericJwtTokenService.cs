using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Business.Services
{
    internal abstract class AGenericJwtTokenService<TClaim, TKey> : IJwtTokenService<TClaim, TKey> where TKey : AsymmetricAlgorithm
    {
        private readonly IJwtTokenGenerator<TClaim> _jwtTokenGenerator;
        private readonly IKeyProvider<TKey> _keyProvider;

        public AGenericJwtTokenService(IJwtTokenGenerator<TClaim> jwtTokenGenerator, IKeyProvider<TKey> keyProvider)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _keyProvider = keyProvider;
        }

        public async Task<byte[]> GetPublicKey()
        {
            try
            {
                TKey rsa = await _keyProvider.GetPrivateKeyAsync();
                return rsa.ExportSubjectPublicKeyInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Internal server error: {ex.Message}");
                return Task.FromResult<byte[]>([]).Result;
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
