namespace MyDiet.Core.Domain.Dtos.Diet
{
    //TODO: Perche esiste? Usa DietDto
    public class CreateDietDto : CreateDietRequest
    {
        public Guid UserId { get; set; }
    }
}
    