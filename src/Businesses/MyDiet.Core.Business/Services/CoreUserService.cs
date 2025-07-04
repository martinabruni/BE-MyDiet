using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class CoreUserService : AGenericService<CoreUserDto, CoreUser, Guid>
    {
        public CoreUserService(IRepository<CoreUser, Guid> repository, IMapper<CoreUser, CoreUserDto> databaseToDtoMapper, IMapper<CoreUserDto, CoreUser> dtoToDatabaseMapper, ResponseMessageOption messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
