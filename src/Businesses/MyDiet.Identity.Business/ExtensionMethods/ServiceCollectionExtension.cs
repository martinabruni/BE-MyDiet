using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Managers;
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
            services.AddTransient<IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto>, KeyVaultSecretService>();
            services.AddTransient<IJwtTokenService<UserClaimDto, RsaSecurityKey>, UserJwtTokenService>();
            services.AddTransient<IJwtTokenManager<UserClaimDto, RsaSecurityKey, JsonWebKeySetDto>, UserJwtTokenManager>();
            return services;
        }
    }
}
