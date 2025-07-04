using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Services
{
    internal class AuthUserService : AGenericService<AuthUserDto, AuthUser, Guid>
    {
        public AuthUserService(IRepository<AuthUser, Guid> repository, IMapper<AuthUser, AuthUserDto> databaseToDtoMapper, IMapper<AuthUserDto, AuthUser> dtoToDatabaseMapper, ResponseMessageOption messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
