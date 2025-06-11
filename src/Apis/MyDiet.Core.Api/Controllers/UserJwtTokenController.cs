using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;

namespace MyDiet.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : ControllerBase
    {
        private readonly IJwtTokenService<UserClaimDto> _jwtTokenService;

        public UserJwtTokenController(IJwtTokenService<UserClaimDto> jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public IActionResult GetToken()
        {
            try
            {
                var userClaim = new UserClaimDto
                {
                    UserId = Guid.NewGuid() // This should be replaced with actual user ID retrieval logic
                };
                var token = _jwtTokenService.GenerateTokenAsync(userClaim).Result;
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
