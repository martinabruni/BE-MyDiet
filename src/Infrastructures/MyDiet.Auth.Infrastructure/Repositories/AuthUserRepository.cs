using BaseUtility;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Infrastructure.Repositories
{
    internal class AuthUserRepository : AGenericRepository<MyDietAuthDbContext, User, Guid>
    {
        public AuthUserRepository(IDatabase<MyDietAuthDbContext> db) : base(db)
        {
        }
        //public override async Task<RepositoryResponse<User>> CreateAsync(User entity)
        //{
        //    //return base.CreateAsync(entity);
        //    if (entity is null)
        //    {
        //        return new RepositoryResponse<User>
        //        {
        //            StatusCode = RepositoryCode.BadRequest,
        //            Message = "Entity cannot be null."
        //        };
        //    }
        //    try
        //    {
        //        //var local = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
        //        //if (local != null)
        //        //{
        //        //    _db.Context.Entry(local).State = EntityState.Detached;
        //        //}

        //        await _dbSet.AddAsync(entity);
        //        await _db.Context.SaveChangesAsync();
        //        return new RepositoryResponse<User>
        //        {
        //            StatusCode = RepositoryCode.Created,
        //            Data = entity,
        //            Message = "Entity created successfully."
        //        };
        //    }
        //    catch
        //    {
        //        return new RepositoryResponse<User>
        //        {
        //            StatusCode = RepositoryCode.InternalServerError,
        //            Message = $"Error creating entity"
        //        };
        //    }
        //}
    }
}
