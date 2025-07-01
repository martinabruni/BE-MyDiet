using MyDiet.Session.Domain.Managers;

namespace System
{
    public static class ServiceProviderExtension
    {
        public static async Task InitializeAsync(this IServiceProvider serviceProvider)
        {
            try
            {
                var keyPairManager = serviceProvider.GetRequiredService<IKeyPairManager>();
                await keyPairManager.RigenerateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
