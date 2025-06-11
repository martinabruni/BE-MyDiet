using MyDiet.Core.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class FoodAlternativeDto : ABaseDto<int>
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
