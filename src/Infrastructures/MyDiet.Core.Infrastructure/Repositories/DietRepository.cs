using BaseUtility;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class DietRepository : BaseRepository<MyDietCoreDbContext, Diet, int>
    {
        public DietRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessage messages) : base(db, messages)
        {
        }
    }
}
