namespace MyDiet.Core.Domain.Dtos
{
    public class MealDto : BaseDto<int>
    {
        public int PlanId { get; set; }

        public int MealTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
