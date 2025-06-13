using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.Interfaces
{
    internal interface IPrivateKeyProvider<TKey> : IKeyProvider where TKey : AsymmetricAlgorithm
    {
        public Task<TKey> GetPrivateKeyAsync();
    }
}
