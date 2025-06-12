using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Security.KeyProviders
{
    internal class LocalKeyProvider : IKeyProvider<RSA>
    {
        public Task<RSA> GetPrivateKeyAsync()
        {
            return Task.FromResult(RSA.Create());
        }
    }
}
