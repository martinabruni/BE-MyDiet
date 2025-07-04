using BaseUtility;

namespace MyDiet.Auth.Domain.Options
{
    public class VaultMessageOption : ResponseMessageOption
    {
        public string EntityPurgedSuccessfully { get; set; } = "The entity has been purged successfully.";
        public string ErrorPurgingEntity { get; set; } = "An error occurred while purging the entity.";
    }
}
