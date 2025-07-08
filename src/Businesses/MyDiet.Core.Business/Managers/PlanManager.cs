using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Options;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class PlanManager : BaseManager<PlanDto, CreatePlanRequest, int>, IManager<PlanDto, CreatePlanRequest, int>
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<PlanDto, Plan, int> _planService;
        private readonly PlanMessageOption _responseMessageOption;
        private readonly IMapper<CreatePlanRequest, PlanDto> _createDtoToPlanDtoMapper;

        public PlanManager(PlanMessageOption responseMessageOption, IService<PlanDto, Plan, int> planService, IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreatePlanRequest, PlanDto> createDtoToPlanDtoMapper)
            : base(userService)
        {
            _responseMessageOption = responseMessageOption;
            _planService = planService;
            _dietService = dietService;
            _createDtoToPlanDtoMapper = createDtoToPlanDtoMapper;
        }

        public async Task<BusinessResponse<PlanDto>> CreateAsync(CreatePlanRequest request, Claim? userIdClaim)
        {
            // Use template method for user and request validation
            var (userId, validationError) = await ValidateUserAndRequestAsync(
                request,
                userIdClaim,
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Validate diet exists and user owns it using indirect ownership validation
            var dietOwnershipError = await ValidateIndirectOwnershipAsync(
                _dietService,
                request.DietId,
                userId,
                diet => diet.UserId,
                _responseMessageOption.EntityNotFound,
                _responseMessageOption.InvalidRequest);

            if (dietOwnershipError != null)
            {
                return dietOwnershipError;
            }

            // Check if plan with same name already exists in the diet
            var existingPlanRes = await _planService.FindAsync(p => p.Name == request.Name && p.DietId == request.DietId);
            if (existingPlanRes.Data is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }
            if (existingPlanRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<PlanDto>.BadRequest(_responseMessageOption.PlanAlreadyExists);
            }

            // Map request to DTO
            var createDto = _createDtoToPlanDtoMapper.Map(request);
            if (createDto is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }

            // Apply creation timestamps using template method
            ApplyCreationTimestamps(createDto);
            
            return await _planService.CreateAsync(createDto);
        }

        public async Task<BusinessResponse<PlanDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            // Use template method for user validation
            var (userId, validationError) = await ValidateUserAsync(
                userIdClaim,
                _responseMessageOption.NotLoggedIn,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get plan entity and validate existence
            var planRes = await ValidateEntityExistsAsync(_planService, id, _responseMessageOption.EntityNotFound);
            if (planRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Validate diet ownership using indirect ownership validation
            var dietOwnershipError = await ValidateIndirectOwnershipAsync(
                _dietService,
                planRes.Data.DietId,
                userId,
                diet => diet.UserId,
                _responseMessageOption.EntityNotFound,
                _responseMessageOption.InvalidRequest);

            if (dietOwnershipError != null)
            {
                return dietOwnershipError;
            }

            return await _planService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<PlanDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            // Use template method for user validation
            var (userId, validationError) = await ValidateUserAsync(
                userIdClaim,
                _responseMessageOption.NotLoggedIn,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get plan entity and validate existence
            var planRes = await ValidateEntityExistsAsync(_planService, id, _responseMessageOption.EntityNotFound);
            if (planRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Validate diet ownership using indirect ownership validation
            var dietOwnershipError = await ValidateIndirectOwnershipAsync(
                _dietService,
                planRes.Data.DietId,
                userId,
                diet => diet.UserId,
                _responseMessageOption.EntityNotFound,
                _responseMessageOption.InvalidRequest);

            if (dietOwnershipError != null)
            {
                return dietOwnershipError;
            }

            return BusinessResponse<PlanDto>.Ok(planRes.Data, _responseMessageOption.EntitiesRetrievedSuccessfully);
        }

        public async Task<BusinessResponse<IEnumerable<PlanDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            // Use template method for user validation
            var (userId, validationError) = await ValidateUserAsync(
                userIdClaim,
                _responseMessageOption.NotLoggedIn,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get all diets for the user
            var dietRes = await _dietService.FindAsync(d => d.UserId == userId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }
            
            var diets = dietRes.Data.ToList();
            if (diets.Count == 0)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Get all diet IDs for the user
            var dietIds = diets.Select(d => d.Id).ToList();

            // Get all plans for these diets
            var plansRes = await _planService.FindAsync(p => dietIds.Contains(p.DietId));
            if (plansRes.Data is null)
            {
                return BusinessResponse<IEnumerable<PlanDto>>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }

            return BusinessResponse<IEnumerable<PlanDto>>.Ok(plansRes.Data, _responseMessageOption.EntitiesRetrievedSuccessfully);
        }

        public async Task<BusinessResponse<PlanDto>> UpdateAsync(CreatePlanRequest request, int id, Claim? userIdClaim)
        {
            // Use template method for user and request validation
            var (userId, validationError) = await ValidateUserAndRequestAsync(
                request,
                userIdClaim,
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Validate diet exists and user owns it using indirect ownership validation
            var dietOwnershipError = await ValidateIndirectOwnershipAsync(
                _dietService,
                request.DietId,
                userId,
                diet => diet.UserId,
                _responseMessageOption.EntityNotFound,
                _responseMessageOption.InvalidRequest);

            if (dietOwnershipError != null)
            {
                return dietOwnershipError;
            }

            // Check if plan with same name already exists in the diet (excluding current plan)
            var planWithSameNameRes = await _planService.FindAsync(p => p.Name == request.Name && p.DietId == request.DietId && p.Id != id);
            if (planWithSameNameRes.Data is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }
            if (planWithSameNameRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<PlanDto>.BadRequest(_responseMessageOption.PlanAlreadyExists);
            }

            // Get current plan to preserve timestamps
            var actualPlan = await ValidateEntityExistsAsync(_planService, id, _responseMessageOption.EntityNotFound);
            if (actualPlan.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Map request to DTO
            var updateDto = _createDtoToPlanDtoMapper.Map(request);
            if (updateDto is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }

            updateDto.Id = id;
            // Apply update timestamps using template method
            ApplyUpdateTimestamps(updateDto, actualPlan.Data.CreatedAt);

            return await _planService.UpdateAsync(updateDto);
        }
    }
}
