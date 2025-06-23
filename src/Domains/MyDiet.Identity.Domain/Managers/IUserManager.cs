using MyDiet.Identity.Domain.Dtos;
using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Managers
{
    public interface IUserManager<TDomain>
        where TDomain : class
    {
        Task<ApiResponse<TDomain>> RegisterAsync(UserRegistrationDto registerDto);
        Task<ApiResponse<TDomain>> LoginAsync(UserLoginDto loginDto);
        Task<ApiResponse<TDomain>> LogoutAsync();
    }
}
