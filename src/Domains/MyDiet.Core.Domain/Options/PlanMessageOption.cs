using BaseUtility;

namespace MyDiet.Core.Domain.Options
{
    public class PlanMessageOption : ResponseMessage
    {
        public string PlanAlreadyExists { get; set; } = "Plan with this name already exists.";
    }
}
