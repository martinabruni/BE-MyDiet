using BaseUtility;
using MyDiet.Auth.Domain.Models;

namespace MyDiet.Auth.Domain.Managers
{
    public interface IKeyPairManager
    {
        Task<BusinessResponse<KeyPair>> RegenerateAsync();
    }
}
