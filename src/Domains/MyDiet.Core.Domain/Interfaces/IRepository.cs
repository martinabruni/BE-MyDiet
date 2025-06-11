using System.Linq.Expressions;

namespace MyDiet.Core.Domain.Interfaces
{
    public interface IRepository<TKey, TDatabase>
    {
        Task<TDatabase> AddAsync(TDatabase entity, CancellationToken ct);

        Task<TDatabase?> UpdateAsync(TDatabase entity, CancellationToken ct);

        Task<TDatabase?> GetByIdAsync(TKey id, CancellationToken ct);

        Task<IEnumerable<TDatabase>> GetAsync(CancellationToken ct, params Expression<Func<TDatabase, bool>>[] filters);

        Task<TDatabase?> DeleteAsync(TKey id, CancellationToken ct);
    }
}
