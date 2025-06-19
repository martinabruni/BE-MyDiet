using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Infrastructure.Repositories;
using MyDiet.Shared.Infrastructure.Converters;
using System.Security.Cryptography;
using System.Text.Json;

namespace Microsoft.Extension.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        private static void ImportRsaKeyParameters(KeyVaultSecret secret, ref RSA rsaKey)
        {
            var _converter = new ByteArrayBase64Converter();
            var _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            _options.Converters.Add(_converter);

            RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(secret.Value, _options);
            rsaKey.ImportParameters(deserializedParameters);
        }

        private static async Task<KeyVaultSecret?> HasPrivateKeyAsync(SecretClient secretClient, string secretName)
        {
            try
            {
                var clientRes = await secretClient.GetSecretAsync(secretName);
                return clientRes.Value;
            }
            catch
            {
                return null;
            }
        }

        private static List<string> GetSecretStringList(IConfiguration configuration, string configurationPath)
        {
            var configurationList = configuration.GetSection(configurationPath).GetChildren().ToList();
            List<string> result = [];
            configurationList.ForEach(item => result.Add(item.Value!));
            return result;
        }

        public static async Task<IServiceCollection> AddJwtInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var rsaKey = RSA.Create(2048);
            var secretName = configuration["Jwt:KeyVault:SecretName"]!;
            var secretClient = new SecretClient(new Uri(configuration["Jwt:KeyVault:Uri"]!), new DefaultAzureCredential());
            var secret = await HasPrivateKeyAsync(secretClient, secretName);

            if (secret is not null)
            {
                ImportRsaKeyParameters(secret, ref rsaKey);
            }

            var issuer = configuration["Jwt:Token:Issuer"]!;
            var algorithmsList = GetSecretStringList(configuration, "Jwt:OpenId:IdTokenSigningAlgorithms");
            services.AddSingleton<SecretClient>(secretClient);
            services.AddSingleton<RSA>(rsaKey);
            services.AddSingleton(sp => new KeyVaultOption
            {
                SecretName = configuration["Jwt:KeyVault:SecretName"]!
            });
            services.AddSingleton<TokenOption>(new TokenOption
            {
                Issuer = issuer,
                Audience = configuration["Jwt:Token:Audience"]!,
                ExpiryMinutes = int.Parse(configuration["Jwt:Token:ExpiryMinutes"]!),
                Algorithm = algorithmsList[0]
            });
            services.AddSingleton(sp =>
                new OpenIdOption
                {
                    Issuer = issuer,
                    AuthorizationEndpoint = configuration["Jwt:OpenId:AuthorizationEndpoint"]!,
                    TokenEndpoint = configuration["Jwt:OpenId:TokenEndpoint"]!,
                    JwksUri = configuration["Jwt:OpenId:JwksUri"]!,
                    IdTokenSigningAlgorithms = algorithmsList,
                    ClaimsSupported = GetSecretStringList(configuration, "Jwt:OpenId:ClaimsSupported")
                }
            );

            services.AddSingleton<ByteArrayBase64Converter>();
            services.AddTransient<IJwtKeyRepository<KeyVaultSecret>, KeyVaultSecretRepository>();
            return services;
        }
    }
}
