using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Models;

namespace MyDiet.Auth.Domain.Managers
{
    public interface IKeyPairManager
    {
        Task<BusinessResponse<KeyPair>> RegenerateAsync();
        Task<BusinessResponse<IEnumerable<RsaSecurityKey>>> GetSigningKeyAsync();
    }
}
