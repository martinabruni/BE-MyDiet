using MyDiet.Core.Business.Services;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {
            services.AddTransient<IJwtTokenService<UserClaimDto, RSA>, UserJwtTokenService>();
            return services;
        }
    }
}
