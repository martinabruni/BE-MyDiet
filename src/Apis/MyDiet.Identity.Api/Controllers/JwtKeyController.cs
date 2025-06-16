using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace MyDiet.Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtKeyController : GenericController
    {
        private readonly IJwtKeyService<RsaSecurityKey, JwkSetDto> _jwtKeyService;
        private readonly IJwtTokenService<UserClaimDto> _jwtTokenService;

        public JwtKeyController(IJwtKeyService<RsaSecurityKey, JwkSetDto> jwtKeyService, IJwtTokenService<UserClaimDto> jwtTokenService)
        {
            _jwtKeyService = jwtKeyService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivateKeyAsync()
        {
            var keyRes = await _jwtKeyService.CreatePrivateKeyAsync();
            return ComposeResult(keyRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKeyAsync()
        {
            var keyRes = await _jwtKeyService.GetPublicKeyAsync();
            return ComposeResult(keyRes);
        }

        [HttpGet]
        public async Task<bool> ValidateToken()
        {
            var userClaim = new UserClaimDto
            {
                UserId = Guid.NewGuid()
            };
            var token = await _jwtTokenService.GenerateTokenAsync(userClaim);
            var publicKey = await _jwtKeyService.GetPublicKeyAsync();

            try
            {
                var rsaParams = new RSAParameters
                {
                    //Correct values
                    Modulus = Base64UrlEncoder.DecodeBytes(publicKey.Data.Keys[0].N),
                    Exponent = Base64UrlEncoder.DecodeBytes(publicKey.Data.Keys[0].E)

                    //Wrong values
                    //Modulus = Base64UrlEncoder.DecodeBytes("sXchRQbfZ0jXWZRC_bNWgzDf0HZzMG2MNF88mKkh6S_SNuqzA3R3SptADMLdj_mTKTg04DybMw4I6Tg5ZJzZEPzcnZ9CMFMjfpTP-Kj3FZFlJpGZ_wG9TKnpgZiv81tYvKhYMQExl47S46DXr9DN3aNXryZkz9D1NFeZz6exJkL8"),
                    //Exponent = Base64UrlEncoder.DecodeBytes("AQAB")
                };

                var securityKey = new RsaSecurityKey(rsaParams)
                {
                    KeyId = publicKey.Data.Keys[0].Kid
                };

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token.Data, tokenValidationParameters, out var validatedToken);
                return true; // Token is valid
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
        }
    }
}
