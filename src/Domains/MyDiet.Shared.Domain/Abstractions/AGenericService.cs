using MyDiet.Shared.Domain.Interfaces;

namespace MyDiet.Shared.Domain.Abstractions
{
    internal class AGenericService<TDto, TDatabase, TKey> : IService<TDto, TKey>
        where TDto : class, IEntity<TKey>
        where TDatabase : class, IEntity<TKey>
        where TKey : notnull
    {
        private readonly IRepository<TDatabase, TKey> _repository;
        private readonly IMapper<TDatabase, TDto> _databaseToDtoMapper;
        private readonly IMapper<TDto, TDatabase> _dtoToDatabaseMapper;

        public AGenericService(IRepository<TDatabase, TKey> repository, IMapper<TDatabase, TDto> databaseToDtoMapper, IMapper<TDto, TDatabase> dtoToDatabaseMapper)
        {
            _repository = repository;
            _databaseToDtoMapper = databaseToDtoMapper;
            _dtoToDatabaseMapper = dtoToDatabaseMapper;
        }

        public async Task<BusinessResponse<TDto>> CreateAsync(TDto dto)
        {
            if (dto is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Entity cannot be null."
                };
            }

            var response = await _repository.CreateAsync(_dtoToDatabaseMapper.Map(dto));

            if (response.Data is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }

            var mappedDto = _databaseToDtoMapper.Map(response.Data);

            return new BusinessResponse<TDto>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDto,
                Message = response.Message
            };
        }

        public async Task<BusinessResponse<TDto>> DeleteAsync(TKey id)
        {
            if (id is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "ID cannot be null."
                };
            }
            var response = await _repository.DeleteAsync(id);
            if (response.Data is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }
            var mappedDto = _databaseToDtoMapper.Map(response.Data);
            return new BusinessResponse<TDto>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDto,
                Message = response.Message
            };
        }

        public async Task<BusinessResponse<IEnumerable<TDto>>> FindAsync(Func<TDto, bool> predicate)
        {
            if (predicate is null)
            {
                return new BusinessResponse<IEnumerable<TDto>>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Predicate cannot be null."
                };
            }
            var response = await _repository.FindAsync(dto => predicate(_databaseToDtoMapper.Map(dto)));
            if (response.Data is null)
            {
                return new BusinessResponse<IEnumerable<TDto>>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }

            //TODO: Centralize this logic in a common method
            if (response.Data.ToList().Count == 0)
            {
                return new BusinessResponse<IEnumerable<TDto>>
                {
                    StatusCode = BusinessCode.Ok,
                    Message = response.Message,
                    Data = []
                };
            }

            var mappedDtos = response.Data.Select(_databaseToDtoMapper.Map);

            return new BusinessResponse<IEnumerable<TDto>>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDtos,
                Message = response.Message
            };
        }

        public async Task<BusinessResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();
            if (response.Data is null)
            {
                return new BusinessResponse<IEnumerable<TDto>>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }

            //TODO: Centralize this logic in a common method
            if (response.Data.ToList().Count == 0)
            {
                return new BusinessResponse<IEnumerable<TDto>>
                {
                    StatusCode = BusinessCode.Ok,
                    Message = response.Message,
                    Data = []
                };
            }
            var mappedDtos = response.Data.Select(_databaseToDtoMapper.Map);
            return new BusinessResponse<IEnumerable<TDto>>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDtos,
                Message = response.Message
            };
        }

        public async Task<BusinessResponse<TDto>> GetByIdAsync(TKey id)
        {
            if (id is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "ID cannot be null."
                };
            }
            var response = await _repository.GetByIdAsync(id);
            if (response.Data is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }
            var mappedDto = _databaseToDtoMapper.Map(response.Data);
            return new BusinessResponse<TDto>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDto,
                Message = response.Message
            };
        }

        public async Task<BusinessResponse<TDto>> UpdateAsync(TDto dto)
        {
            if (dto is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Entity cannot be null."
                };
            }
            var response = await _repository.UpdateAsync(_dtoToDatabaseMapper.Map(dto));
            if (response.Data is null)
            {
                return new BusinessResponse<TDto>
                {
                    StatusCode = (BusinessCode)response.StatusCode,
                    Message = response.Message
                };
            }
            var mappedDto = _databaseToDtoMapper.Map(response.Data);
            return new BusinessResponse<TDto>
            {
                StatusCode = (BusinessCode)response.StatusCode,
                Data = mappedDto,
                Message = response.Message
            };
        }
    }
}
