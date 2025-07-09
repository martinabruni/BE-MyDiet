using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Managers;
using System.Security.Claims;

namespace MyDiet.Shared.Api.Controllers.Core
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PlanController : ControllerBase
    {
        private readonly IManager<PlanDto, CreatePlanRequest, int> _planManager;

        public PlanController(IManager<PlanDto, CreatePlanRequest, int> planManager)
        {
            _planManager = planManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanByIdAsync([FromQuery] int id)
        {
            var response = await _planManager.GetByIdAsync(id, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlanAsync([FromBody] CreatePlanRequest planDto)
        {
            var response = await _planManager.CreateAsync(planDto, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPlansAsync()
        {
            var response = await _planManager.GetByUserIdAsync(User.GetUserId());
            return response.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlanAsync([FromBody] CreatePlanRequest planDto, [FromQuery] int id)
        {
            var response = await _planManager.UpdateAsync(planDto, id, User.GetUserId());
            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlanAsync([FromQuery] int id)
        {
            var response = await _planManager.DeleteAsync(id, User.GetUserId());
            return response.ToActionResult();
        }
    }
}
