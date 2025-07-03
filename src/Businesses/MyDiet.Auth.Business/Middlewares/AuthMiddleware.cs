using BaseUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyDiet.Auth.Business.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IVaultService<JsonWebKeySetDto> publicKeyService, IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> keyPairMapper, TokenOption tokenOption)
    {
        // 1. Check for Bearer token
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            // No token, skip
            await _next(context);
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        // 2. Get public key
        var publicKeyResponse = await publicKeyService.GetAsync();
        if (publicKeyResponse.Data == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Public key not found.");
            return;
        }

        // 3. Map to RsaSecurityKey
        var rsaSecurityKey = keyPairMapper.Map(publicKeyResponse.Data);

        // 4. Validate token
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsaSecurityKey.FirstOrDefault(),
            ValidIssuer = tokenOption.Issuer
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
            // Token is valid, continue
            await _next(context);
        }
        catch (SecurityTokenException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid token.");
        }
    }
}
