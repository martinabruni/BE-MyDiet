using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Infrastructure;
using MyDiet.Identity.Infrastructure.Interfaces;
using MyDiet.Identity.Infrastructure.JwtTokenGenerators;
using MyDiet.Identity.Infrastructure.KeyProviders;
using System.Security.Cryptography;

namespace Microsoft.Extension.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
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
                    PrivateKeyPath = configuration["JwtSettings:PrivateKeyPath"],
                    //ClientId = configuration["JwtSettings:ClientId"],
                    //TenantId = configuration["JwtSettings:TenantId"],
                    //ClientSecret = configuration["JwtSettings:ClientSecret"]
                }
            );
            return services;
        }

        public static IServiceCollection AddKeyProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtSettings(configuration);
            services.AddSingleton<IPrivateKeyProvider<RSA>, LocalKeyProvider>();
            return services;
        }

        public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenGenerator<UserClaimDto>, UserJwtTokenGenerator>();
            return services;
        }
    }
}
