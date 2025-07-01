using BaseUtility;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Managers;

namespace MyDiet.Auth.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest request)
        {
            var response = await _authManager.RegisterUserAsync(request);
            return response.ToActionResult();
        }
    }
}
