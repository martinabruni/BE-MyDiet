using MyDiet.Auth.Domain.Managers;

namespace System
{
    public static class ServiceProviderExtension
    {
        public static async Task InitializeAsync(this IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var keyPairManager = scope.ServiceProvider.GetRequiredService<IKeyPairManager>();
                await keyPairManager.RigenerateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
