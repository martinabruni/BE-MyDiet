using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Infrastructure.Repositories
{
    internal class UserRepository : AGenericRepository<User, Guid>
    {
        public UserRepository(IDatabase db) : base(db)
        {
        }
    }
}
