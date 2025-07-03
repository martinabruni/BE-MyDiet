using BaseUtility;

namespace MyDiet.Shared.Domain.Dtos
{
    public class DietDto : BaseDto<int>
    {
        public Guid UserId { get; set; }

        public required string Name { get; set; }
    }
}
