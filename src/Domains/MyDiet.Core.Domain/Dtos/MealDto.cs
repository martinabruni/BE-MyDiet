using MyDiet.Core.Domain.Abstractions;

namespace MyDiet.Core.Domain.Dtos
{
    public class MealDto : ABaseDto<int>
    {
        public int PlanId { get; set; }

        public int MealTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
