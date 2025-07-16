using BaseUtility;

namespace MyDiet.Core.Infrastructure.Models
{
    public partial class Diet : IAuditable, IEntity<int>, IAuthorizedEntity<Guid?>
    {
    }
}
