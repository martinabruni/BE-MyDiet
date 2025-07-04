using MyDiet.Shared.Business.Mappers;
using MyDiet.Shared.Business.Services;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommonBusiness(this IServiceCollection services)
        {
            services.AddSingleton<IMapper<User, UserDto>, UserMapper>();
            services.AddSingleton<IMapper<UserDto, User>, UserMapper>();

            services.AddTransient<IService<UserDto, User, Guid>, UserService>();
            return services;
        }
    }
}
