using BaseUtility;

namespace MyDiet.Core.Domain.Dtos.Diet
{
    public class DietDto : BaseDto<int>
    {
        public Guid UserId { get; set; }

        public required string Name { get; set; }
    }
}
