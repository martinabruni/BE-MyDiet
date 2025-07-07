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
using MyDiet.Core.Domain.Dtos.ForeignKeys;

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
            services.AddScoped<IMapper<Plan, PlanDto>, PlanMapper>();
            services.AddScoped<IMapper<PlanDto, Plan>, PlanMapper>();
            services.AddScoped<IMapper<CreatePlanRequest, PlanDto>, PlanMapper>();

            services.AddSingleton<DietMessageOption>();
            services.AddSingleton<PlanMessageOption>();
            services.AddScoped<IService<DietDto, Diet, int>, DietService>();
            services.AddScoped<IService<PlanDto, Plan, int>, PlanService>();
            services.AddScoped<IService<CoreUserDto, CoreUser, Guid>, CoreUserService>();

            services.AddScoped<IManager<DietDto, CreateDietRequest, int>, DietManager>();
            services.AddScoped<IManager<PlanDto, CreatePlanRequest, int>, PlanManager>();
            return services;
        }
    }
}
