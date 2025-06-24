using BaseUtility;
using MyDiet.Auth.Business.Managers;
using MyDiet.Auth.Business.Mappers;
using MyDiet.Auth.Business.Services;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Infrastructure.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthBusiness(this IServiceCollection services)
        {
            services.AddScoped<IMapper<AuthUserDto, User>, AuthUserMapper>();
            services.AddScoped<IMapper<User, AuthUserDto>, AuthUserMapper>();

            services.AddScoped<IService<AuthUserDto, Guid>, AuthUserService>();
            services.AddScoped<IAuthManager, AuthManager>();
            return services;
        }
    }
}
