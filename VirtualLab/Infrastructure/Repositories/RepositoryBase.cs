using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProxmoxApi;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.ApiResult;

namespace VirtualLab.Infrastructure.Repositories;
// так интересно, что чем больше контекста, тем вероятнее нужно будет менять навзвание разных элементов,
// просто потому что, что было конкертным из-за усложнее контекста становится уже не на столько конкертно))
// моя философская мысль, в obsidian позже закину.
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

    public async Task<Result> Update(TEntity entity)
    {
        var response = await _dbContext.Set<TEntity>().FindAsync(entity.Id);
        if (response == null) return Result.Fail(ErrorDb.NotFound(typeof(TEntity).Name, entity.Id.ToString()));
        _dbContext.Entry(response).CurrentValues.SetValues(entity);
        _dbContext.Entry(response).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    } 
}