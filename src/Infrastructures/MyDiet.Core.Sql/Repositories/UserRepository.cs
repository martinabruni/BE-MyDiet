using Microsoft.EntityFrameworkCore;
using MyDiet.Core.Sql.Abstractions;
using MyDiet.Core.Sql.Models;

namespace MyDiet.Core.Sql.Repositories
{
    internal class UserRepository : AGenericRepository<Guid, User>
    {
        public UserRepository(MyDietCoreDbContext dbContext, DbSet<User> dbSet) : base(dbContext, dbSet)
        {
        }
    }
}
