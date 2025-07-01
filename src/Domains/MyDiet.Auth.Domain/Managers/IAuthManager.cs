using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;

namespace MyDiet.Auth.Domain.Managers
{
    public interface IAuthManager
    {
        Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request);
        Task<BusinessResponse<TokenResponse>> LoginUserAsync(UserLoginRequest request);
        Task<BusinessResponse<TokenResponse>> LogoutUserAsync(string token);

    }
}
