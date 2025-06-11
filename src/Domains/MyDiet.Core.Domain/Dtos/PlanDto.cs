using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class PlanDto : BaseDto<int>
    {
        [Required]
        public int DietId { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
