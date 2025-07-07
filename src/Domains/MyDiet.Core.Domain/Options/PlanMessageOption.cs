using BaseUtility;

namespace MyDiet.Core.Domain.Options
{
    public class PlanMessageOption : ResponseMessageOption
    {
        public string PlanAlreadyExists { get; set; } = "Plan with this name already exists.";
    }
}
