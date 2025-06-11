using MyDiet.Core.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class CalendarMealDto : ABaseDto<int>
    {
        [Required]
        public int CalendarEntryId { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
