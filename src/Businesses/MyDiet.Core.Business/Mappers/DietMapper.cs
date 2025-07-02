using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Mappers
{
    internal class DietMapper : IMapper<Diet, DietDto>, IMapper<DietDto, Diet>
    {
        public Diet Map(DietDto input)
        {
            return new Diet
            {
                Id = input.Id,
                UserId = input.UserId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public DietDto Map(Diet input)
        {
            return new DietDto
            {
                Id = input.Id,
                UserId = input.UserId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }
    }
}
