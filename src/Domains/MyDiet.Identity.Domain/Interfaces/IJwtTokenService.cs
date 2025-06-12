using MyDiet.Shared.Domain.Responses;
using System.Security.Cryptography;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtTokenService<TClaim, TKey> where TKey : AsymmetricAlgorithm
    {
        Task<ApiDataResponse<string>> GetPemPublicKey();
        Task<ApiDataResponse<string>> GetPublicKeyAsync();
        Task<ApiDataResponse<string>> GenerateTokenAsync(TClaim claimDto);
    }
}
