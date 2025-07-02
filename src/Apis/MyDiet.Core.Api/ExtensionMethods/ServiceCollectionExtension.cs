using BaseUtility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyDiet.Auth.Domain.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
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
                    //TODO: please change this ASAP and make it work
                    // Fetch the signing key from the KeyPairController endpoint
                    var httpClient = new HttpClient();
                    // Adjust the URL to your actual Auth API host and port
                    var signingKeyResponse = httpClient.GetFromJsonAsync<BusinessResponse<IEnumerable<RsaSecurityKey>>>(
                        "https://localhost:7113/api/KeyPair/GetSigningKeyAsync").GetAwaiter().GetResult();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:7113",
                        IssuerSigningKeys = signingKeyResponse.Data
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
            return services;
        }
        public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreInfrastructure(configuration);
            services.AddCoreBusiness();
            services.AddTokenValidation();
            services.AddCoreBusiness();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
