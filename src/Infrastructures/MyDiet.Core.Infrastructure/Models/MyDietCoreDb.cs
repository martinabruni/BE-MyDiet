using BaseUtility;

namespace MyDiet.Core.Infrastructure.Models
{
    internal class MyDietCoreDb : IDatabase<MyDietCoreDbContext>
    {
        private readonly MyDietCoreDbContext _context;

        public MyDietCoreDb(MyDietCoreDbContext context)
        {
            _context = context;
        }

        public MyDietCoreDbContext Context { get => _context; }
    }
}
