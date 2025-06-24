using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Options;
using MyDiet.Session.Domain.Repositories;
using MyDiet.Session.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        private static List<string> GetSecretStringList(IConfiguration configuration, string configurationPath)
        {
            var configurationList = configuration.GetSection(configurationPath).GetChildren().ToList();
            List<string> result = [];
            configurationList.ForEach(item => result.Add(item.Value));
            return result;
        }

        public static IServiceCollection AddJwtInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

            services.AddTransient<IVaultRepository<KeyVaultSecret>, PrivateKeyRepository>();
            services.AddSingleton<KeyPair>();
            return services;
        }
    }
}