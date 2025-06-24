using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Responses;

namespace MyDiet.Session.Domain.Managers
{
    public interface IKeyPairManager
    {
        Task<BusinessResponse<KeyPair>> RigenerateAsync();
    }
}
