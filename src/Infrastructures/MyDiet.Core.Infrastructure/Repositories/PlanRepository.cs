using BaseUtility;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class PlanRepository : BaseRepository<MyDietCoreDbContext, Plan, int>
    {
        public PlanRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessage messages) : base(db, messages)
        {
        }
    }
}
