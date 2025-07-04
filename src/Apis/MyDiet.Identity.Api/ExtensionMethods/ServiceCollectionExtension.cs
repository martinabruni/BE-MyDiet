namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddCommonInfrastructure(configurations);
            services.AddJwtInfrastructure(configurations.GetSection("Jwt"));
            services.AddCommonBusiness();
            services.AddIdentityBusiness();
            services.AddJwtServices();
            return services;
        }
    }
}
