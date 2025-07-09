using BaseUtility;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class CoreUserService : BaseService<CoreUserDto, CoreUser, Guid>
    {
        public CoreUserService(IRepository<CoreUser, Guid> repository, IMapper<CoreUser, CoreUserDto> databaseToDtoMapper, IMapper<CoreUserDto, CoreUser> dtoToDatabaseMapper, ResponseMessage messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
