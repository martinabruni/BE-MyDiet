using MyDiet.Core.Business.Services;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService<UserClaimDto>, UserJwtTokenService>();
            return services;
        }
    }
}
