using MyDiet.Shared.Domain.Interfaces;

namespace MyDiet.Shared.Domain.Dtos
{
    public abstract class BaseDto<TKey> : IEntity<TKey>
        where TKey : notnull
    {
        public required TKey Id { get; set; }
    }
}
