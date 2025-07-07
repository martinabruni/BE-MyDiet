using BaseUtility;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class PlanRepository : AGenericRepository<MyDietCoreDbContext, Plan, int>
    {
        public PlanRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessageOption messages) : base(db, messages)
        {
        }
    }
}
