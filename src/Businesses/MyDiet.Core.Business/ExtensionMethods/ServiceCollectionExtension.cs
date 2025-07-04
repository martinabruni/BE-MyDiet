using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Core.Business.Managers;
using MyDiet.Core.Business.Mappers;
using MyDiet.Core.Business.Services;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Options;
using MyDiet.Core.Infrastructure.Models;

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
            services.AddScoped<IMapper<CreateDietDto, DietDto>, DietMapper>();
            services.AddScoped<IMapper<CreateDietRequest, CreateDietDto>, DietMapper>();

            services.AddSingleton<DietManagerMessageOption>();
            services.AddScoped<IService<DietDto, Diet, int>, DietService>();
            services.AddScoped<IService<CoreUserDto, CoreUser, Guid>, CoreUserService>();

            services.AddScoped<IDietManager, DietManager>();
            return services;
        }
    }
}
