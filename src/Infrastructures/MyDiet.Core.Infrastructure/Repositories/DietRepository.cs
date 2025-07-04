using BaseUtility;
using Microsoft.EntityFrameworkCore;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Infrastructure.Repositories
{
    internal class DietRepository : AGenericRepository<MyDietCoreDbContext, Diet, int>
    {
        public DietRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessageOption messages) : base(db, messages)
        {
        }
    }
}
