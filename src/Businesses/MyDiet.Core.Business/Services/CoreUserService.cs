using BaseUtility;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Services
{
    internal class CoreUserService : AGenericService<CoreUserDto, CoreUser, Guid>
    {
        public CoreUserService(IRepository<CoreUser, Guid> repository, IMapper<CoreUser, CoreUserDto> databaseToDtoMapper, IMapper<CoreUserDto, CoreUser> dtoToDatabaseMapper) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper)
        {
        }
    }
}
