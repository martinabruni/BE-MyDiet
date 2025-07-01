namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeyPairInfrastructure(configuration);
            services.AddKeyPairBusiness();
            services
                .AddAuthentication()
                .AddBearerToken();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddAuthInfrastructure(configuration);
            services.AddAuthBusiness();

            return services;
        }
    }
}
