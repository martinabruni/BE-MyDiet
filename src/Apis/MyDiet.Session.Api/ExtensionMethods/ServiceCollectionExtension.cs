namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtInfrastructure(configuration);
            services.AddSessionBusinessServices();
            return services;
        }
    }
}
