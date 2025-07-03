using BaseUtility;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Managers
{
    internal class DietManager
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService)
        {
            _userService = userService;
            _dietService = dietService;
        }


    }
}
