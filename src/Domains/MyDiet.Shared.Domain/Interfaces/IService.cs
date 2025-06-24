namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IService<TDto, TKey>
        where TDto : class, IEntity<TKey>
        where TKey : notnull
    {
        Task<BusinessResponse<TDto>> GetByIdAsync(TKey id);
        Task<BusinessResponse<TDto>> CreateAsync(TDto dto);
        Task<BusinessResponse<TDto>> UpdateAsync(TDto dto);
        Task<BusinessResponse<TDto>> DeleteAsync(TKey id);
        Task<BusinessResponse<IEnumerable<TDto>>> GetAllAsync();
        Task<BusinessResponse<IEnumerable<TDto>>> FindAsync(Func<TDto, bool> predicate);
    }
}
