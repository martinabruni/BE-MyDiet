namespace MyDiet.Shared.Domain
{
    public interface IEntity<TKey>
        where TKey : notnull
    {
        TKey Id { get; set; }
    }
}
