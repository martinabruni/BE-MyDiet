using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Managers;
using MyDiet.Identity.Business.Managers.Jwt;
using MyDiet.Identity.Business.Mappers;
using MyDiet.Identity.Business.Services;
using MyDiet.Identity.Business.Services.Jwt;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Managers;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Domain.Services;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityBusiness(this IServiceCollection services)
        {
            services.AddSingleton<IMapper<UserRegistrationDto, IdentityUserDto>, IdentityUserMapper>();
            services.AddSingleton<IMapper<User, IdentityUserDto>, IdentityUserMapper>();
            services.AddSingleton<IMapper<IdentityUserDto, User>, IdentityUserMapper>();

            services.AddTransient<IService<IdentityUserDto, User, Guid>, IdentityUserService>();
            services.AddTransient<IUserManager<IdentityUserDto>, UserManager>();

            return services;
        }
        public static IServiceCollection AddJwtServices(this IServiceCollection services)
        {
            services.AddSingleton<IMapper<KeyVaultSecret, JsonWebKeySetDto>, KeyVaultSecretMapper>();
            services.AddSingleton<IMapper<KeyVaultSecret, RsaSecurityKey>, KeyVaultSecretMapper>();
            services.AddSingleton<IMapper<RSA, KeyVaultSecret>, KeyVaultSecretMapper>();
            services.AddSingleton<IMapper<OpenIdOption, OpenIdConfigurationDto>, OpenIdMapper>();
            services.AddSingleton<ByteArrayBase64Converter>();

            services.AddTransient<IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto>, VaultSecretService>();
            services.AddTransient<IJwtTokenService<UserClaims, RsaSecurityKey>, JwtTokenService>();
            services.AddTransient<IJwtTokenManager<UserRegistrationDto, UserClaims, RsaSecurityKey, JsonWebKeySetDto>, UserJwtTokenManager>();
            return services;
        }
    }
}
