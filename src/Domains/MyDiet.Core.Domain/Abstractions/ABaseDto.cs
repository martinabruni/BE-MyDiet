using MyDiet.Core.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyDiet.Core.Domain.Abstractions
{
    public abstract class ABaseDto<TKey> : IEntity<TKey>
    {
        [Key]
        [Required]
        public TKey Id { get; set; }
    }
}
