using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProxmoxApi;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.ApiResult;

namespace VirtualLab.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> where TEntity : class, IEntity<Guid>
{
    protected DbContext _dbContext;
    protected WithDataBase ErrorDb => ApiResultError.WithDataBase;

    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Result<TEntity>> Get(TId id)
    {
        var response = await _dbContext.Set<TEntity>().FindAsync(id);

        return response.ExistOrFail();
    }

    public async Task<Result> Insert(TEntity entity)
    {
        var entityEntry = await _dbContext.Set<TEntity>().AddAsync(entity);
        if (entityEntry.State != EntityState.Added)
        {
            ErrorDb.NotInsert(typeof(TEntity).Name, entity.Id.ToString(), "entity had been added already");
        }

        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result<IReadOnlyCollection<TEntity>>> GetAll()
    {
        var entities = await _dbContext.Set<TEntity>().ToArrayAsync();
        
        return entities;
    }
}