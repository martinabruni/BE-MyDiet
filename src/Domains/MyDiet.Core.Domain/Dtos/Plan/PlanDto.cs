using BaseUtility;

namespace MyDiet.Core.Domain.Dtos.Plan
{
    public class PlanDto : BaseDto<int>
    {
        public required int DietId { get; set; }

        public required string Name { get; set; }
    }
}
