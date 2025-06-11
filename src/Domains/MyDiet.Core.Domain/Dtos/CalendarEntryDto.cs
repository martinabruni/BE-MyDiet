using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class CalendarEntryDto : BaseDto<int>
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
