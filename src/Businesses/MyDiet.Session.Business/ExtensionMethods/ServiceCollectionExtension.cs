using Azure.Security.KeyVault.Secrets;
using MyDiet.Session.Business.Managers;
using MyDiet.Session.Business.Mappers;
using MyDiet.Session.Business.Services;
using MyDiet.Session.Domain.Managers;
using MyDiet.Session.Domain.Mappers;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Services;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSessionBusinessServices(this IServiceCollection services)
        {
            services.AddSingleton<ByteArrayBase64Converter>();

            services.AddScoped<IMapper<KeyVaultSecret, JsonWebKeySetDto>, KeyPairMapper>();
            services.AddScoped<IMapper<RSA, KeyVaultSecret>, KeyPairMapper>();

            services.AddScoped<IVaultService<KeyVaultSecret>, PrivateKeyService>();
            services.AddScoped<IVaultService<JsonWebKeySetDto>, PublicKeyService>();

            services.AddScoped<IKeyPairManager, KeyPairManager>();

            return services;
        }
    }
}
