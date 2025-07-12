using BaseUtility;

namespace MyDiet.Core.Domain.Dtos.Diet
{
    public class DietDto : BaseDto<int>, IAuthorizedEntity<Guid>
    {
        public Guid UserId { get; set; }

        public required string Name { get; set; }
    }
}
