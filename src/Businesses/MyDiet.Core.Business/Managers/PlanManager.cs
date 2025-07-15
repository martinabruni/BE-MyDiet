using BaseUtility;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Business.ValidationPipelines;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Mappers;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class PlanManager : BaseManager<CreatePlanRequest>, IManager<PlanDto, CreatePlanRequest, int>
    {
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<PlanDto, Plan, int> _planService;
        private readonly ResponseMessage _message;
        private readonly IMapper<CreatePlanRequest, PlanDto> _createDtoToPlanDtoMapper;
        private readonly PlanValidationPipelineSet _pipelineSet;
        private readonly ContextProvider<CoreValidationContext<PlanDto, int>> _contextProvider = new() { Context = new() };
        private readonly IAsyncMapper<ContextProvider<CoreValidationContext<PlanDto, int>>, ContextProvider<CoreValidationContext<DietDto, int>>> _contextMapper;

        public PlanManager(ResponseMessage message, IService<PlanDto, Plan, int> planService, IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreatePlanRequest, PlanDto> createDtoToPlanDtoMapper, PlanValidationPipelineSet pipelineSet, IAsyncMapper<ContextProvider<CoreValidationContext<PlanDto, int>>, ContextProvider<CoreValidationContext<DietDto, int>>> contextMapper)
        {
            _message = message;
            _planService = planService;
            _userService = userService;
            _dietService = dietService;
            _createDtoToPlanDtoMapper = createDtoToPlanDtoMapper;
            _pipelineSet = pipelineSet;
            _contextMapper = contextMapper;
        }

        public async Task<BusinessResponse<PlanDto>> CreateAsync(CreatePlanRequest request, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<PlanDto, int>
            {
                Data = new PlanDto
                {
                    DietId = request.DietId,
                    Id = 0,
                    Name = request.Name,
                    UserId = new()
                },
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.CreationValidators.ValidateAsync(request, _contextProvider);

            var createDto = validationRes.Data;

            if (createDto is null)
            {
                return validationRes;
            }

            return await _planService.CreateAsync(createDto);
        }

        public async Task<BusinessResponse<PlanDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<PlanDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.DeletionValidators.ValidateAsync(id, _contextProvider);
            if (validationRes.StatusCode != BusinessCode.Ok)
            {
                return validationRes;
            }

            return await _planService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<PlanDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<PlanDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationResult = await _pipelineSet.GetByIdValidators.ValidateAsync(id, _contextProvider);
            if (validationResult.Data is null)
            {
                return validationResult;
            }

            //TODO: chiamata duplicata
            return validationResult;
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
            var planDto = new PlanDto
            {
                DietId = request.DietId,
                Name = request.Name,
                Id = id,
                UserId = new()
            };

            _contextProvider.Context = new CoreValidationContext<PlanDto, int>
            {
                Data = planDto,
                UserClaim = userIdClaim
            };

            var validationResult = await _pipelineSet.UpdateValidators.ValidateAsync(planDto, _contextProvider);

            if (validationResult.StatusCode != BusinessCode.Ok)
            {
                return validationResult;
            }

            return await _planService.UpdateAsync(validationResult.Data);
        }
    }
}
