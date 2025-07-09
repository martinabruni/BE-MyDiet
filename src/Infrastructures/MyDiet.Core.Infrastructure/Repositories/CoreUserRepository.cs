using BaseUtility;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class CoreUserRepository : BaseRepository<MyDietCoreDbContext, CoreUser, Guid>
    {
        public CoreUserRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessage messages) : base(db, messages)
        {
        }
    }
}
