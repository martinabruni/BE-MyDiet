using BaseUtility;

namespace MyDiet.Core.Domain.Options
{
    public class DietManagerMessageOption : ResponseMessageOption
    {
        public string DietAlreadyExists { get; set; } = "Diet already exists.";
    }
}
