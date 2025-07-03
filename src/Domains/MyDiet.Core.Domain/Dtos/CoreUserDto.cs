using BaseUtility;

namespace MyDiet.Shared.Domain.Dtos
{
    public class CoreUserDto : BaseDto<Guid>
    {
        public string? Username { get; set; }
        //TODO: add role
    }
}
