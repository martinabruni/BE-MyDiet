using BaseUtility;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Infrastructure.Repositories
{
    internal class AuthUserRepository : AGenericRepository<MyDietAuthDbContext, AuthUser, Guid>
    {
        public AuthUserRepository(IDatabase<MyDietAuthDbContext> db) : base(db)
        {
        }
    }
}
