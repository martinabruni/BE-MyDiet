using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Services
{
    internal class AuthUserService : AGenericService<AuthUserDto, User, Guid>
    {
        public AuthUserService(IRepository<User, Guid> repository, IMapper<User, AuthUserDto> databaseToDtoMapper, IMapper<AuthUserDto, User> dtoToDatabaseMapper) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper)
        {
        }
    }
}
