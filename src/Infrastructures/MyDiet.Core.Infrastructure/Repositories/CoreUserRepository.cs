using BaseUtility;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Infrastructure.Repositories
{
    internal class CoreUserRepository : AGenericRepository<MyDietCoreDbContext, CoreUser, Guid>
    {
        public CoreUserRepository(IDatabase<MyDietCoreDbContext> db) : base(db)
        {
        }
    }
}
