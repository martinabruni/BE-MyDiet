using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.KeyProviders
{
    internal class LocalKeyProvider : IKeyProvider<RSA>
    {
        private readonly RSA _rsa;

        public LocalKeyProvider()
        {
            _rsa = RSA.Create();
        }

        public Task<RSA> GetPrivateKeyAsync()
        {
            return Task.FromResult(_rsa);
        }
    }
}
