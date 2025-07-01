using BaseUtility;

namespace MyDiet.Auth.Infrastructure.Models
{
    internal class MyDietAuthDb : IDatabase<MyDietAuthDbContext>
    {
        private readonly MyDietAuthDbContext _context;

        public MyDietAuthDb(MyDietAuthDbContext context)
        {
            _context = context;
        }

        public MyDietAuthDbContext Context { get => _context; }
    }
}
