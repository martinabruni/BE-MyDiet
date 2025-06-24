namespace MyDiet.Shared.Domain
{
    public abstract class BaseDto<TKey> : IEntity<TKey>, IAuditable
        where TKey : notnull
    {
        public required TKey Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
