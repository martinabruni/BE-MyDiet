using BaseUtility;
using MyDiet.Session.Domain.Models;

namespace MyDiet.Session.Domain.Managers
{
    public interface IKeyPairManager
    {
        Task<BusinessResponse<KeyPair>> RigenerateAsync();
    }
}
