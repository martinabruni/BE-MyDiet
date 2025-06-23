using Microsoft.EntityFrameworkCore;
using MyDiet.Shared.Domain.Interfaces;

namespace MyDiet.Shared.Infrastructure.Models
{
    public partial class MyDietDbContext : IDatabase
    {
        public DbContext Context => this;
    }
}
