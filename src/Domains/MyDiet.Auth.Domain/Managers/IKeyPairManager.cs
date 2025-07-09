using BaseUtility;
using MyDiet.Auth.Domain.Dtos;

namespace MyDiet.Auth.Domain.Managers
{
    public interface IKeyPairManager
    {
        Task<BusinessResponse<KeyPair>> RegenerateAsync();
    }
}
