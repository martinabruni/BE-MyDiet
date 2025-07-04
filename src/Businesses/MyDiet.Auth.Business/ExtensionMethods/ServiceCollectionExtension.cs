using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Business.Managers;
using MyDiet.Auth.Business.Mappers;
using MyDiet.Auth.Business.Services;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;
using MyDiet.Auth.Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKeyPairBusiness(this IServiceCollection services)
        {
            services.AddSingleton<ByteArrayBase64Converter>();

            services.AddScoped<IMapper<KeyVaultSecret, JsonWebKeySetDto>, KeyPairMapper>();
            services.AddScoped<IMapper<RSA, KeyVaultSecret>, KeyPairMapper>();
            services.AddScoped<IMapper<KeyVaultSecret, RsaSecurityKey>, KeyPairMapper>();
            services.AddScoped<IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>>, KeyPairMapper>();

            services.AddSingleton<KeyPairMessageOption>();
            services.AddScoped<IVaultService<KeyVaultSecret>, PrivateKeyService>();
            services.AddScoped<IVaultService<JsonWebKeySetDto>, PublicKeyService>();

            services.AddScoped<IKeyPairManager, KeyPairManager>();

            return services;
        }

        public static IServiceCollection AddAuthBusiness(this IServiceCollection services)
        {
            services.AddScoped<IMapper<AuthUserDto, AuthUser>, AuthUserMapper>();
            services.AddScoped<IMapper<AuthUser, AuthUserDto>, AuthUserMapper>();
            services.AddScoped<IMapper<UserRegistrationRequest, AuthUserDto>, AuthUserMapper>();
            services.AddScoped<IMapper<AuthUserDto, UserRegistrationResponse>, AuthUserMapper>();
            services.AddScoped<IMapper<AuthUserDto, UserClaims>, AuthUserMapper>();
            services.AddScoped<IMapper<UserClaims, List<Claim>>, ClaimMapper>();
            services.AddScoped<IMapper<JwtSecurityToken, TokenResponse>, TokenMapper>();

            services.AddScoped<IService<AuthUserDto, AuthUser, Guid>, AuthUserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton<AuthManagerMessageOption>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ITokenManager, TokenManager>();
            return services;
        }
    }
}
