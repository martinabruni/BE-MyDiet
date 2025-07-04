using MyDiet.Identity.Domain.Dtos;
using MyDiet.Shared.Business.Services;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Identity.Business.Services
{
    internal class IdentityUserService : AGenericService<IdentityUserDto, User, Guid>, IService<IdentityUserDto, User, Guid>
    {
        public IdentityUserService(IRepository<User, Guid> repository, IMapper<User, IdentityUserDto> databaseToDomainMapper, IMapper<IdentityUserDto, User> domaintToDatabaseMapper) : base(repository, databaseToDomainMapper, domaintToDatabaseMapper)
        {
        }
    }
}
