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
            services.AddSingleton<RSA>(_rsaKey);
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
                    SigningCredentials = new SigningCredentials(new RsaSecurityKey(_rsaKey), SecurityAlgorithms.RsaSha256)
                }
            );
            services.AddSingleton(sp => new SecretClient(new Uri(configuration["Jwt:KeyVault:Uri"]!), new DefaultAzureCredential()));

            services.AddTransient<IJwtKeyRepository<KeyVaultSecret>, KeyVaultSecretRepository>();

            return services;
        }
    }
}
