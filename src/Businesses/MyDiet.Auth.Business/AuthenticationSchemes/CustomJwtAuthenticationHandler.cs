using BaseUtility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;

namespace MyDiet.Auth.Business.AuthenticationSchemes;
public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
    private readonly IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> _keyPairMapper;
    private readonly TokenOption _tokenOption;

    public CustomJwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IVaultService<JsonWebKeySetDto> publicKeyService,
        IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> keyPairMapper,
        TokenOption tokenOption)
        : base(options, logger, encoder, clock)
    {
        _publicKeyService = publicKeyService;
        _keyPairMapper = keyPairMapper;
        _tokenOption = tokenOption;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return AuthenticateResult.NoResult();

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var publicKeyResponse = await _publicKeyService.GetAsync();
        if (publicKeyResponse.Data is null)
            return AuthenticateResult.Fail("Signing key not found.");

        var rsaSecurityKey = _keyPairMapper.Map(publicKeyResponse.Data).FirstOrDefault();
        if (rsaSecurityKey is null)
            return AuthenticateResult.Fail("No valid signing key.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsaSecurityKey,
            ValidIssuer = _tokenOption.Issuer
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (SecurityTokenException ex)
        {
            return AuthenticateResult.Fail("Invalid token");
        }
    }
}
