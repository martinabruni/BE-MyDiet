using System.Security.Cryptography;

namespace MyDiet.Core.Domain.Interfaces
{
    public interface IJwtTokenService<TClaim, TKey> where TKey : AsymmetricAlgorithm
    {
        Task<string> GetPublicKey();
        Task<string> GenerateTokenAsync(TClaim claimDto);
    }
}
