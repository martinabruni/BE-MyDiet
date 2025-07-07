using MyDiet.Core.Domain.Dtos.Requests;

namespace MyDiet.Core.Domain.Dtos.ForeignKeys
{
    public class CreateDietDto : CreateDietRequest
    {
        public Guid UserId { get; set; }
    }
}
