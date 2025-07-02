using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class DietService : AGenericService<DietDto, Diet, int>
    {
        public DietService(IRepository<Diet, int> repository, IMapper<Diet, DietDto> databaseToDtoMapper, IMapper<DietDto, Diet> dtoToDatabaseMapper) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper)
        {
        }
    }
}
