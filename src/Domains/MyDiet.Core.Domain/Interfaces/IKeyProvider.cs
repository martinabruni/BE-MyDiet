using System.Security.Cryptography;

namespace MyDiet.Core.Domain.Interfaces
{
    public interface IKeyProvider<TKey> where TKey : AsymmetricAlgorithm
    {
        Task<TKey> GetKeyAsync();
    }
}
