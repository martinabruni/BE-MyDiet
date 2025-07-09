using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Mappers
{
    internal class PlanMapper : IMapper<Plan, PlanDto>, IMapper<PlanDto, Plan>, IMapper<CreatePlanRequest, PlanDto>
    {
        public Plan Map(PlanDto input)
        {
            return new Plan
            {
                Id = input.Id,
                DietId = input.DietId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public PlanDto Map(Plan input)
        {
            return new PlanDto
            {
                Id = input.Id,
                DietId = input.DietId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public PlanDto Map(CreatePlanRequest input)
        {
            return new PlanDto
            {
                Id = 0, // Assuming Id is auto-generated
                DietId = input.DietId,
                Name = input.Name,
            };
        }
    }
}
