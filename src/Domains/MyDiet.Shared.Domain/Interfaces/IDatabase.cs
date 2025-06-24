using Microsoft.EntityFrameworkCore;

namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IDatabase<TContext>
        where TContext : DbContext
    {
        TContext Context { get; }
    }
}
