using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.KeyProviders
{
    internal class LocalKeyProvider : IKeyProvider<RSA>
    {
        public Task<RSA> GetPrivateKeyAsync()
        {
            return Task.FromResult(RSA.Create());
        }
    }
}
