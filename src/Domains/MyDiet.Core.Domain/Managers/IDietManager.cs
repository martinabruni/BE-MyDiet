using BaseUtility;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Shared.Domain.Dtos;
using System.Security.Claims;

namespace MyDiet.Core.Domain.Managers
{
    public interface IDietManager
    {
        Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim);

        Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim);

        Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim);

        Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim);

        Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim);
    }
}
