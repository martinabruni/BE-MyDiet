using BaseUtility;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class CoreUserRepository : AGenericRepository<MyDietCoreDbContext, CoreUser, Guid>
    {
        public CoreUserRepository(IDatabase<MyDietCoreDbContext> db) : base(db)
        {
        }
    }
}
