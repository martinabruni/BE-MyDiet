using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Services;

namespace MyDiet.Identity.Api.Controllers.Jwt
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JwtKeyController : ControllerBase
    {
        private readonly IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> _jwtKeyService;

        public JwtKeyController(ILogger<JwtKeyController> logger, IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> jwtKeyService)
        {
            _jwtKeyService = jwtKeyService;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegenerateKeyPairAsync()
        {
            var keyRes = await _jwtKeyService.RegenerateKeyPairAsync();
            return this.ComposeResult(keyRes);
        }


        [HttpGet]
        public IActionResult GetPublicKey()
        {
            var keyRes = _jwtKeyService.GetPublicKey();
            return this.ComposeResult(keyRes);
        }


        [HttpGet("~/.well-known/openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var openIdConfigurationRes = _jwtKeyService.GetOpenIdConfigurationKey();
            return this.ComposeResult(openIdConfigurationRes);
        }
    }
}
