using BaseUtility;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Infrastructure.Repositories
{
    internal class AuthUserRepository : BaseRepository<MyDietAuthDbContext, AuthUser, Guid>
    {
        public AuthUserRepository(IDatabase<MyDietAuthDbContext> db, ResponseMessage messages) : base(db, messages)
        {
        }
    }
}
