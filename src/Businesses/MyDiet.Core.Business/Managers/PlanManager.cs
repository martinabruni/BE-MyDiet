using BaseUtility;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class PlanManager : BaseManager<CreatePlanRequest>, IManager<PlanDto, CreatePlanRequest, int>
    {
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<PlanDto, Plan, int> _planService;
        private readonly PlanMessage _message;
        private readonly IMapper<CreatePlanRequest, PlanDto> _createDtoToPlanDtoMapper;

        public PlanManager(PlanMessage message, IService<PlanDto, Plan, int> planService, IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreatePlanRequest, PlanDto> createDtoToPlanDtoMapper)
        {
            _message = message;
            _planService = planService;
            _userService = userService;
            _dietService = dietService;
            _createDtoToPlanDtoMapper = createDtoToPlanDtoMapper;
        }

        public async Task<BusinessResponse<PlanDto>> CreateAsync(CreatePlanRequest request, Claim? userIdClaim)
        {
            var validationResult = ValidateAndGetUserId(request, userIdClaim);
            if (validationResult is null)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.InvalidRequest);
            }

            var userId = (Guid)validationResult;
            var userRes = await _userService.GetByIdAsync(userId);
            if (userRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(request.DietId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }

            var existingPlanRes = await _planService.FindAsync(p => p.Name == request.Name && p.DietId == request.DietId);
            if (existingPlanRes.Data is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_message.ErrorRetrievingEntities);
            }
            if (existingPlanRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.PlanAlreadyExists);
            }
            var createDto = _createDtoToPlanDtoMapper.Map(request);
            if (createDto is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_message.ErrorCreatingEntity);
            }
            createDto.CreatedAt = DateTime.UtcNow;
            createDto.UpdatedAt = createDto.CreatedAt;
            return await _planService.CreateAsync(createDto);
        }

        public async Task<BusinessResponse<PlanDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            var validationResult = ValidateUserClaim(userIdClaim);
            if (validationResult is null)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.NotLoggedIn);
            }
            var userId = (Guid)validationResult;
            var planRes = await _planService.GetByIdAsync(id);
            if (planRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(planRes.Data.DietId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }
            return await _planService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<PlanDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            var validationResult = ValidateUserClaim(userIdClaim);
            if (validationResult is null)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.NotLoggedIn);
            }
            var userId = (Guid)validationResult;
            var planRes = await _planService.GetByIdAsync(id);
            if (planRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            var dietRes = await _dietService.GetByIdAsync(planRes.Data.DietId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }
            return BusinessResponse<PlanDto>.Ok(planRes.Data, _message.EntitiesRetrievedSuccessfully);
        }

        public async Task<BusinessResponse<IEnumerable<PlanDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            var validationResult = ValidateUserClaim(userIdClaim);
            if (validationResult is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.BadRequest(_message.NotLoggedIn);
            }
            var userId = (Guid)validationResult;
            var userRes = await _userService.GetByIdAsync(userId);
            if (userRes.Data is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.NotFound(_message.EntityNotFound);
            }
            var dietRes = await _dietService.FindAsync(d => d.UserId == userId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.InternalServerError(_message.ErrorRetrievingEntities);
            }
            var diets = dietRes.Data.ToList();
            if (diets.Count == 0)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.NotFound(_message.EntityNotFound);
            }

            // Get all diet IDs for the user
            var dietIds = diets.Select(d => d.Id).ToList();

            // Get all plans for these diets
            var plansRes = await _planService.FindAsync(p => dietIds.Contains(p.DietId));
            if (plansRes.Data is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.InternalServerError(_message.ErrorRetrievingEntities);
            }

            return BusinessResponse<IEnumerable<PlanDto>>.Ok(plansRes.Data, _message.EntitiesRetrievedSuccessfully);
        }

        public async Task<BusinessResponse<PlanDto>> UpdateAsync(CreatePlanRequest request, int id, Claim? userIdClaim)
        {
            var validationResult = ValidateAndGetUserId(request, userIdClaim);
            if (validationResult is null)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.InvalidRequest);
            }
            var userId = (Guid)validationResult;
            var userRes = await _userService.GetByIdAsync(userId);
            if (userRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            var dietRes = await _dietService.GetByIdAsync(request.DietId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }
            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }
            var planWithSameNameRes = await _planService.FindAsync(p => p.Name == request.Name && p.DietId == request.DietId && p.Id != id);
            if (planWithSameNameRes.Data is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_message.ErrorRetrievingEntities);
            }
            if (planWithSameNameRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<PlanDto>.BadRequest(_message.PlanAlreadyExists);
            }
            var actualPlan = await _planService.GetByIdAsync(id);

            if (actualPlan.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }

            var updateDto = _createDtoToPlanDtoMapper.Map(request);
            if (updateDto is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_message.ErrorCreatingEntity);
            }

            updateDto.Id = id;
            updateDto.CreatedAt = actualPlan.Data.CreatedAt;
            updateDto.UpdatedAt = DateTime.UtcNow;

            return await _planService.UpdateAsync(updateDto);
        }
    }
}
