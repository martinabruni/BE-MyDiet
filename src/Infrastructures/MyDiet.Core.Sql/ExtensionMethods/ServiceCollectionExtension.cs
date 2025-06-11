namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreSql(this IServiceCollection services)
        {
            // TODO: Register db context
            //services.AddDbContext<MyDietCoreDbContext>

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // TODO: Register repositories
            return services;
        }
    }
}
