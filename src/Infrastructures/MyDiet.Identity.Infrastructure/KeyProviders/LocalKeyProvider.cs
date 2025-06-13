using MyDiet.Identity.Infrastructure.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Infrastructure.KeyProviders
{
    internal class LocalKeyProvider : IPrivateKeyProvider<RSA>
    {
        private readonly RSA _rsa;

        public LocalKeyProvider()
        {
            _rsa = RSA.Create(2048);
        }

        public Task<RSA> GetPrivateKeyAsync()
        {
            return Task.FromResult(_rsa);
        }

        public Task<string> GetPublicKeyAsync()
        {
            var publicKey = _rsa.ExportSubjectPublicKeyInfo();
            return Task.FromResult(PemEncoding.WriteString("PUBLIC KEY", publicKey));
        }
    }
}
