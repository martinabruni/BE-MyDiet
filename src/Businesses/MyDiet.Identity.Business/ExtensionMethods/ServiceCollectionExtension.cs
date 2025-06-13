using MyDiet.Identity.Business.Services;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {
            services.AddTransient<IJwtTokenService<UserClaimDto>, UserJwtTokenService>();
            return services;
        }
    }
}
