using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Domain.Repositories;
using MyDiet.Shared.Infrastructure.Repositories;

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

        public static IServiceCollection AddJwtInfrastructure(this IServiceCollection services, IConfiguration configurations)
        {
            var issuer = configurations["Token:Issuer"];
            var algorithm = configurations["Jwk:Alg"];

            services.AddSingleton<SecretClient>(new SecretClient(new Uri(configurations["KeyVault:Uri"]), new DefaultAzureCredential()));
            services.AddSingleton(sp => new KeyVaultOption
            {
                PrivateKeyName = configurations["KeyVault:SecretName"],
                KeySize = int.Parse(configurations["KeyVault:KeySize"]),
            });
            services.AddSingleton<TokenOption>(new TokenOption
            {
                Issuer = issuer,
                Audience = configurations["Token:Audience"],
                ExpiryMinutes = int.Parse(configurations["Token:ExpiryMinutes"]),
                Algorithm = algorithm
            });
            services.AddSingleton<JwkOption>(new JwkOption
            {
                Kty = configurations["Jwk:Kty"],
                Use = configurations["Jwk:Use"],
                Alg = algorithm
            });
            services.AddSingleton(sp =>
                new OpenIdOption
                {
                    Issuer = issuer,
                    AuthorizationEndpoint = configurations["OpenId:AuthorizationEndpoint"],
                    TokenEndpoint = configurations["OpenId:TokenEndpoint"],
                    JwksUri = configurations["OpenId:JwksUri"],
                    IdTokenSigningAlgorithms = [algorithm],
                    ClaimsSupported = GetSecretStringList(configurations, "OpenId:ClaimsSupported")
                }
            );

            services.AddTransient<IVaultRepository<KeyVaultSecret, Response>, VaultSecretRepository>();
            services.AddSingleton<KeyPairDto>();
            return services;
        }
    }
}
