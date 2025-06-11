using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Dtos
{
    public class BaseDto<TKey>
    {
        [Key]
        [Required]
        public TKey Id { get; set; }
    }
}
