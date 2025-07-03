using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyDiet.Auth.Business.AuthenticationSchemes;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        private static IServiceCollection AddTokenValidation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Example: \"Bearer yourtoken\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            services
                .AddAuthentication("CustomJwt")
                .AddScheme<AuthenticationSchemeOptions, CustomJwtAuthenticationHandler>("CustomJwt", null);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
            return services;
        }

        private static IServiceCollection AddAuthStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeyPairInfrastructure(configuration);
            services.AddKeyPairBusiness();
            services.AddAuthInfrastructure(configuration);
            services.AddAuthBusiness();
            services.AddTokenValidation();

            return services;
        }

        private static IServiceCollection AddCoreStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreInfrastructure(configuration);
            services.AddCoreBusiness();
            return services;
        }

        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthStartupServices(configuration);
            services.AddCoreStartupServices(configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
