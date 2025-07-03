using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Api.Controllers.Core
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DietController : ControllerBase
    {
        private readonly IService<DietDto, Diet, int> _dietService;

        public DietController(IService<DietDto, Diet, int> dietService)
        {
            _dietService = dietService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietAsync([FromBody] DietDto dietDto)
        {
            var response = await _dietService.CreateAsync(dietDto);
            return response.ToActionResult();
        }
    }
}
