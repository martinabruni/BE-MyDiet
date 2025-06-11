using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using MyDiet.Core.Security;
using MyDiet.Core.Security.JwtTokenGenerators;
using MyDiet.Core.Security.KeyProviders;
using System.Security.Cryptography;

namespace Microsoft.Extension.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKeyProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
                new JwtSettings
                {
                    Issuer = configuration["JwtSettings:Issuer"] ?? "localhost",
                    Audience = configuration["JwtSettings:Audience"] ?? "myapp-api",
                    ExpiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60"),
                    UseKeyVault = bool.Parse(configuration["JwtSettings:UseKeyVault"] ?? "false"),
                    VaultUri = configuration["JwtSettings:VaultUri"],
                    KeyName = configuration["JwtSettings:KeyName"],
                    PrivateKeyPath = configuration["JwtSettings:PrivateKeyPath"]
                }
            );
            services.AddSingleton<IKeyProvider<RSA>, AzureKeyVaultKeyProvider>();
            return services;
        }

        public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator<UserClaimDto>, UserJwtTokenGenerator>();
            return services;
        }
    }
}
