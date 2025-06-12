using System.Security.Cryptography;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IKeyProvider<TKey> where TKey : AsymmetricAlgorithm
    {
        Task<TKey> GetPrivateKeyAsync();
    }
}
