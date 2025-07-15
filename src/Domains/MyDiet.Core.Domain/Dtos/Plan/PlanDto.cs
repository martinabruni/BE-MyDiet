using BaseUtility;

namespace MyDiet.Core.Domain.Dtos.Plan
{
    public class PlanDto : BaseDto<int>, IAuthorizedEntity<Guid>
    {
        public required int DietId { get; set; }

        public required string Name { get; set; }

        public required Guid UserId { get; set; }
    }
}
