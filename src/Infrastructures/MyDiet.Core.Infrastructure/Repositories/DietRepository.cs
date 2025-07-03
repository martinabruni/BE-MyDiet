using BaseUtility;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Infrastructure.Repositories
{
    internal class DietRepository : AGenericRepository<MyDietCoreDbContext, Diet, int>
    {
        public DietRepository(IDatabase<MyDietCoreDbContext> db) : base(db)
        {
        }
    }
}
