using MyDiet.Identity.Domain;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Managers;
using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Domain.Responses;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Identity.Business.Managers
{
    public class UserManager : IUserManager<IdentityUserDto>
    {
        private readonly IService<IdentityUserDto, User, Guid> _userService;
        private readonly IMapper<UserRegistrationDto, IdentityUserDto> _userMapper;

        public UserManager(IService<IdentityUserDto, User, Guid> userService, IMapper<UserRegistrationDto, IdentityUserDto> userMapper)
        {
            _userService = userService;
            _userMapper = userMapper;
        }

        public Task<ApiResponse<IdentityUserDto>> LoginAsync(UserLoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IdentityUserDto>> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IdentityUserDto>> RegisterAsync(UserRegistrationDto registerDto)
        {
            return await _userService.AddAsync(_userMapper.Map(registerDto));
        }
    }
}
