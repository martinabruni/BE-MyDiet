using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Shared.Business.Mappers;
using MyDiet.Shared.Business.Services;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreBusiness(this IServiceCollection services)
        {
            services.AddScoped<IMapper<Diet, DietDto>, DietMapper>();
            services.AddScoped<IMapper<DietDto, Diet>, DietMapper>();
            services.AddScoped<IMapper<CoreUser, CoreUserDto>, CoreUserMapper>();
            services.AddScoped<IMapper<CoreUserDto, CoreUser>, CoreUserMapper>();
            services.AddScoped<IMapper<UserClaims, CoreUserDto>, CoreUserMapper>();
            services.AddScoped<IService<DietDto, Diet, int>, DietService>();
            services.AddScoped<IService<CoreUserDto, CoreUser, Guid>, CoreUserService>();
            return services;
        }
    }
}
