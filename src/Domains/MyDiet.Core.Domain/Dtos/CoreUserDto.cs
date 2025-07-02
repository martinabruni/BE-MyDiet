using BaseUtility;

namespace MyDiet.Core.Domain.Dtos
{
    public class CoreUserDto : BaseDto<Guid>
    {
        public string? Username { get; set; }
    }
}
