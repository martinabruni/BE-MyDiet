using MyDiet.Core.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class FoodDto : ABaseDto<int>
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
