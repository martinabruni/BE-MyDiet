using Microsoft.AspNetCore.Mvc;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Managers;

namespace MyDiet.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager<IdentityUserDto> _userManager;

        public UserController(IUserManager<IdentityUserDto> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registerDto)
        {
            var response = await _userManager.RegisterAsync(registerDto);
            return this.ComposeResult(response);
        }
    }
}
