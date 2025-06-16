using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Infrastructure.Repositories;
using System.Security.Cryptography;

namespace Microsoft.Extension.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var _rsaKey = RSA.Create(2048);
            var _rsaSecurityKey = new RsaSecurityKey(_rsaKey) { KeyId = configuration["Jwt:Token:KeyId"]! };
            services.AddSingleton<RSA>(_rsaKey);
            services.AddSingleton<RsaSecurityKey>(_rsaSecurityKey);
            services.AddSingleton(sp => new KeyVaultOption
            {
                SecretName = configuration["Jwt:KeyVault:SecretName"] ?? "privateKey"
            });
            services.AddSingleton(sp =>
                new TokenOption
                {
                    Issuer = configuration["Jwt:Token:Issuer"]!,
                    Audience = configuration["Jwt:Token:Audience"]!,
                    ExpiryMinutes = int.Parse(configuration["Jwt:Token:ExpiryMinutes"]!),
                    SigningCredentials = new SigningCredentials(_rsaSecurityKey, SecurityAlgorithms.RsaSha256)
                }
            );
            services.AddSingleton(sp => new SecretClient(new Uri(configuration["Jwt:KeyVault:Uri"]!), new DefaultAzureCredential()));

            services.AddTransient<IJwtKeyRepository<KeyVaultSecret>, KeyVaultSecretRepository>();

            return services;
        }
    }
}
