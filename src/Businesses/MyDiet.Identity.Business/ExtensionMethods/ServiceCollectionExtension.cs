using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Converters;
using MyDiet.Identity.Business.Services;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddJwtBusiness(this IServiceCollection services)
        {
            services.AddSingleton<ByteArrayBase64Converter>();
            services.AddTransient<IJwtKeyService<RsaSecurityKey, JwkSetDto>, KeyVaultSecretService>();
            services.AddTransient<IJwtTokenService<UserClaimDto>, UserJwtTokenService>();
            return services;
        }
    }
}
