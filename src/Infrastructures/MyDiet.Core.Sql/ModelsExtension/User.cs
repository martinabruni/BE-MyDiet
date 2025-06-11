using MyDiet.Core.Domain.Interfaces;

namespace MyDiet.Core.Sql.Models;

public partial class User : IAuditable, IEntity<Guid>
{
}