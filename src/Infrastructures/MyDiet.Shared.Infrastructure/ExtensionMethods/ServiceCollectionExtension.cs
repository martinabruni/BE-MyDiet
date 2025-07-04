using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Infrastructure.Models;
using MyDiet.Shared.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services, IConfiguration configurations)
        {
            //TODO: add connection string to key vault
            services.AddDbContext<IDatabase, MyDietDbContext>(options =>
            {
                options.UseSqlServer(configurations.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<IRepository<User, Guid>, UserRepository>();
            return services;
        }
    }
}
