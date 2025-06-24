using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Managers;

namespace MyDiet.Auth.Business.Managers
{
    internal class AuthManager : IAuthManager
    {
        private readonly IService<AuthUserDto, Guid> _authUserService;

        public AuthManager(IService<AuthUserDto, Guid> authUserService)
        {
            _authUserService = authUserService;
        }


        //public async Task<BusinessResponse<>> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        //{

        //}
    }
}
