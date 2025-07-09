namespace MyDiet.Core.Domain.Dtos.Plan
{
    public class CreatePlanRequest
    {
        public required string Name { get; set; }
        public required int DietId { get; set; }
    }
}
