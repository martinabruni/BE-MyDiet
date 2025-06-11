using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class FoodAlternativeDto : BaseDto<int>
    {
        [Required]
        public int FoodId { get; set; }

        [Required]
        public int AlternativeFoodId { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
