using BaseUtility;

namespace MyDiet.Core.Domain.Options
{
    public class DietMessageOption : ResponseMessageOption
    {
        public string DietAlreadyExists { get; set; } = "Diet already exists.";
    }
}
