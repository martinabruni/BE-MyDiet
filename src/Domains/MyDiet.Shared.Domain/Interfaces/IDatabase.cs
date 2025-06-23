using Microsoft.EntityFrameworkCore;

namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IDatabase
    {
        DbContext Context { get; }
    }
}
