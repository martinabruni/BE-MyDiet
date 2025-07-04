using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using System.Security.Claims;

namespace MyDiet.Core.Api.Controllers.Core
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DietController : ControllerBase
    {
        private readonly IDietManager _dietManager;

        public DietController(IDietManager dietManager)
        {
            _dietManager = dietManager;
        }

        private static Claim? GetUserIdFromClaims(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "userId");
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietAsync([FromBody] CreateDietRequest dietDto)
        {
            var response = await _dietManager.CreateAsync(dietDto, GetUserIdFromClaims(User));
            return response.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetDietByIdAsync([FromQuery] int id)
        {
            var response = await _dietManager.GetByIdAsync(id, GetUserIdFromClaims(User));
            return response.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDietAsync([FromBody] CreateDietRequest dietDto, [FromQuery] int id)
        {
            var response = await _dietManager.UpdateAsync(dietDto, id, GetUserIdFromClaims(User));
            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDietAsync([FromQuery] int id)
        {
            var response = await _dietManager.DeleteAsync(id, GetUserIdFromClaims(User));
            return response.ToActionResult();
        }
    }
}
