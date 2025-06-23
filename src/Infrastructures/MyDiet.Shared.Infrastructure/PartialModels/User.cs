using MyDiet.Shared.Domain.Interfaces;

namespace MyDiet.Shared.Infrastructure.Models
{
    public partial class User : IAuditable, IEntity<Guid>
    {
    }
}
