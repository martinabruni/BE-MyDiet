namespace MyDiet.Session.Domain.Mappers
{
    public interface IMapper<TInput, TOutput> where TInput : class where TOutput : class
    {
        TOutput Map(TInput input);
    }
}
