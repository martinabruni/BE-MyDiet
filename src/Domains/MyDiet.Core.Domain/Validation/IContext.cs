namespace MyDiet.Core.Domain.Validation
{
    public interface IContext<TData>
        where TData : class
    {
        /// <summary>
        /// Gets the data associated with the context.
        /// </summary>
        TData Data { get; }
    }
}
