namespace MyDiet.Core.Domain.Mappers
{
    public interface IAsyncMapper<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        Task<TOutput> Map(TInput input);
    }
}
