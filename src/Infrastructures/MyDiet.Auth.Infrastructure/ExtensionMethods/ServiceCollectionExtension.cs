using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Repositories;
using MyDiet.Auth.Infrastructure.Models;
using MyDiet.Auth.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKeyPairInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var algorithm = jwtSection["Jwk:Alg"];

            services.AddSingleton(new SecretClient(new Uri(configuration["Vault:Uri"]), new DefaultAzureCredential()));
            services.AddSingleton(new KeyOption
            {
                PrivateKeyName = configuration["Key:PrivateKeyName"],
                KeySize = int.Parse(configuration["Key:KeySize"]),
            });
            services.AddSingleton(new JsonWebKeyOption
            {
                Kty = jwtSection["Jwk:Kty"],
                Use = jwtSection["Jwk:Use"],
                Alg = algorithm
            });

            services.AddScoped<IVaultRepository<KeyVaultSecret>, PrivateKeyRepository>();
            services.AddSingleton<KeyPair>();
            return services;
        }
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSection = configuration.GetSection("Token");

            services.AddDbContext<MyDietAuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDatabase<MyDietAuthDbContext>, MyDietAuthDb>();
            services.AddScoped<IRepository<User, Guid>, AuthUserRepository>();
            services.AddSingleton(new TokenOption
            {
                Algorithm = tokenSection["Algorithm"],
                Audience = tokenSection["Audience"],
                ExpiryMinutes = int.Parse(tokenSection["ExpiryMinutes"]),
                Issuer = tokenSection["Issuer"]
            });
            return services;
        }
    }
}
