using MyDiet.Shared.Domain.Interfaces;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Domain.Responses;
using System.Linq.Expressions;
using System.Net;

namespace MyDiet.Shared.Business.Services
{
    public class AGenericService<TDomain, TEntity, TKey> : IService<TDomain, TEntity, TKey>
        where TDomain : class
        where TEntity : class, IEntity<TKey>, IAuditable
        where TKey : notnull
    {
        private readonly IRepository<TEntity, TKey> _repository;
        private readonly IMapper<TEntity, TDomain> _databaseToDomainMapper;
        private readonly IMapper<TDomain, TEntity> _domaintToDatabaseMapper;

        public AGenericService(IRepository<TEntity, TKey> repository, IMapper<TEntity, TDomain> databaseToDomainMapper, IMapper<TDomain, TEntity> domaintToDatabaseMapper)
        {
            _repository = repository;
            _databaseToDomainMapper = databaseToDomainMapper;
            _domaintToDatabaseMapper = domaintToDatabaseMapper;
        }

        private List<TDomain> GetDtos(IEnumerable<TEntity> entities)
        {

            List<TDomain> dtos = [];
            entities.ToList().ForEach(entity => dtos.Add(_databaseToDomainMapper.Map(entity)));
            return dtos;
        }

        public async Task<ApiResponse<TDomain>> AddAsync(TDomain? entity)
        {
            try
            {
                if (entity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Entity cannot be null."
                    };
                }
                var newEntity = _domaintToDatabaseMapper.Map(entity);
                newEntity.CreatedAt = DateTime.UtcNow;

                await _repository.AddAsync(newEntity);
                if (newEntity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = "Failed to add entity."
                    };
                }
                return new ApiResponse<TDomain>()
                {
                    Data = _databaseToDomainMapper.Map(newEntity),
                    StatusCode = HttpStatusCode.Created,
                    Message = "Entity added successfully."
                };
            }
            catch
            {
                return new ApiResponse<TDomain>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to add entity."
                };
            }
        }

        public async Task<ApiResponse<TDomain>> DeleteAsync(TKey id)
        {
            try
            {
                var deletedEntity = await _repository.DeleteAsync(id);
                if (deletedEntity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Entity not found."
                    };
                }
                return new ApiResponse<TDomain>()
                {
                    Data = _databaseToDomainMapper.Map(deletedEntity),
                    StatusCode = HttpStatusCode.OK,
                    Message = "Entity deleted successfully."
                };
            }
            catch
            {
                return new ApiResponse<TDomain>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to delete entity."
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TDomain>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();

                return new ApiResponse<IEnumerable<TDomain>>()
                {
                    Data = GetDtos(entities),
                    StatusCode = HttpStatusCode.OK,
                    Message = "Entities retrieved successfully."
                };
            }
            catch
            {
                return new ApiResponse<IEnumerable<TDomain>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to retrieve entities."
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<TDomain>>> GetAsync(Expression<Func<TEntity, bool>>? filters = null)
        {
            try
            {
                var entities = await _repository.GetAsync(filters);
                return new ApiResponse<IEnumerable<TDomain>>()
                {
                    Data = GetDtos(entities),
                    StatusCode = HttpStatusCode.OK,
                    Message = "Entities retrieved successfully."
                };
            }
            catch
            {
                return new ApiResponse<IEnumerable<TDomain>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to retrieve entities."
                };
            }
        }

        public async Task<ApiResponse<TDomain>> GetByIdAsync(TKey id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Entity not found."
                    };
                }
                return new ApiResponse<TDomain>()
                {
                    Data = _databaseToDomainMapper.Map(entity),
                    StatusCode = HttpStatusCode.OK,
                    Message = "Entity retrieved successfully."
                };
            }
            catch
            {
                return new ApiResponse<TDomain>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to retrieve entity."
                };
            }
        }

        public async Task<ApiResponse<TDomain>> UpdateAsync(TDomain? entity)
        {
            try
            {
                if (entity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Entity cannot be null."
                    };
                }
                var updatedEntity = _domaintToDatabaseMapper.Map(entity);
                updatedEntity.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(updatedEntity);

                if (updatedEntity is null)
                {
                    return new ApiResponse<TDomain>()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = "Failed to update entity."
                    };
                }
                return new ApiResponse<TDomain>()
                {
                    Data = _databaseToDomainMapper.Map(updatedEntity),
                    StatusCode = HttpStatusCode.OK,
                    Message = "Entity updated successfully."
                };
            }
            catch
            {
                return new ApiResponse<TDomain>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to update entity."
                };
            }
        }
    }
}
