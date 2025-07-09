using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Managers;
using System.Security.Claims;

namespace MyDiet.Shared.Api.Controllers.Core
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DietController : ControllerBase
    {
        private readonly IManager<DietDto, CreateDietRequest, int> _dietManager;

        public DietController(IManager<DietDto, CreateDietRequest, int> dietManager)
        {
            _dietManager = dietManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietAsync([FromBody] CreateDietRequest dietDto)
        {
            var response = await _dietManager.CreateAsync(dietDto, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetDietByIdAsync([FromQuery] int id)
        {
            var response = await _dietManager.GetByIdAsync(id, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDietAsync([FromBody] CreateDietRequest dietDto, [FromQuery] int id)
        {
            var response = await _dietManager.UpdateAsync(dietDto, id, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDietAsync([FromQuery] int id)
        {
            var response = await _dietManager.DeleteAsync(id, User.GetUserId());
            return response.ToActionResult();
        }
    }
}
