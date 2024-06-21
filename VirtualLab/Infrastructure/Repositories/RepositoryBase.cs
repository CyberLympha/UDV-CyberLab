using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProxmoxApi;
using VirtualLab.Domain.Interfaces.Repositories;

namespace VirtualLab.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> where TEntity : class
{
    protected DbContext _dbContext;

    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TEntity>> Get(TId id)
    {
        var response = await _dbContext.Set<TEntity>().FindAsync(id);

        return
            response.AssertExistsOrFail(); // а должна ли быть вообще ошибка при если у пользователя и нету данных по id??
    }

    public async Task<Result> Insert(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyCollection<TEntity>>> GetAll()
    {
        var entities = await _dbContext.Set<TEntity>().ToArrayAsync();

        return entities;
    }
}