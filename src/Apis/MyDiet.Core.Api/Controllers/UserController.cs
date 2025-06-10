using Microsoft.AspNetCore.Mvc;

namespace MyDiet.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser()
        {
            return Ok("Hello from UserController!");
        }
    }
}
