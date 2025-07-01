using BaseUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyDiet.Auth.Infrastructure.Models;
using MyDiet.Auth.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDietAuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDatabase<MyDietAuthDbContext>, MyDietAuthDb>();
            services.AddScoped<IRepository<User, Guid>, AuthUserRepository>();
            return services;
        }
    }
}
