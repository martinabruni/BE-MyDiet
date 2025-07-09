using BaseUtility;

namespace MyDiet.Core.Domain.Dtos.CoreUser
{
    public class CoreUserDto : BaseDto<Guid>
    {
        public string? Username { get; set; }
        //TODO: add role
    }
}
