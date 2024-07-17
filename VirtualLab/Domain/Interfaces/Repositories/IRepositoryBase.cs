using FluentResults;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IRepositoryBase<TEntity, in TId>
{
    public Task<Result<TEntity>> Get(TId id);
    public Task<Result> Insert(TEntity entity);

    public Task<Result> Update(TEntity entity);
    
    public Task<Result<IReadOnlyCollection<TEntity>>> GetAll();
}