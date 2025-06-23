using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Services
{
    internal class UserService : AGenericService<UserDto, User, Guid>, IService<UserDto, User, Guid>
    {
        public UserService(IRepository<User, Guid> repository, IMapper<User, UserDto> databaseToDomainMapper, IMapper<UserDto, User> domaintToDatabaseMapper) : base(repository, databaseToDomainMapper, domaintToDatabaseMapper)
        {
        }
    }
}
