using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Services;

namespace System
{
    public static class ServiceProviderExtension
    {
        public static async Task InitializeAsync(this IServiceProvider serviceProvider)
        {
            var keyService = serviceProvider.GetRequiredService<IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto>>();
            var keyServiceRes = keyService.GetPrivateKey();
            if (keyServiceRes.Data is null)
            {
                await keyService.RegenerateKeyPairAsync();
            }
        }

        public static async Task UseWarmUpAsync(this IServiceProvider serviceProvider)
        {
            await serviceProvider.InitializeAsync();
        }
    }
}
