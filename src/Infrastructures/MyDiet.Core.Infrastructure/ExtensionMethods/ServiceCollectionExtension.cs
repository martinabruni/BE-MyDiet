using BaseUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyDiet.Core.Infrastructure.Models;
using MyDiet.Core.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDietCoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDatabase<MyDietCoreDbContext>, MyDietCoreDb>();
            services.AddScoped<IRepository<CoreUser, Guid>, CoreUserRepository>();
            services.AddScoped<IRepository<Diet, int>, DietRepository>();
            services.AddScoped<IRepository<Plan, int>, PlanRepository>();

            return services;
        }
    }
}
