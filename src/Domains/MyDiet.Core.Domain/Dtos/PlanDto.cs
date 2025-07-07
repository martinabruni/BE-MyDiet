using BaseUtility;

namespace MyDiet.Core.Domain.Dtos
{
    public class PlanDto : BaseDto<int>
    {
        public required int DietId { get; set; }

        public required string Name { get; set; }
    }
}
