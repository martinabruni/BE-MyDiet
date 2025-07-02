using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MyDiet.Auth.Domain.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        //TODO: centralize this method somewhere else
        public static IServiceCollection AddTokenValidation(this IServiceCollection services)
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
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(async options =>
                {
                    var tokenManager = services.BuildServiceProvider().GetRequiredService<ITokenManager>();
                    var parameters = await tokenManager.GetValidationParametersAsync();
                    options.TokenValidationParameters = parameters.Data;
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
            return services;
        }

        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeyPairInfrastructure(configuration);
            services.AddKeyPairBusiness();
            services.AddAuthInfrastructure(configuration);
            services.AddAuthBusiness();
            services.AddTokenValidation();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
