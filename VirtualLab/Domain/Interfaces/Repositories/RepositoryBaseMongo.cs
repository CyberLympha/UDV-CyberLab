using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public abstract class RepositoryBaseMongo : IRepositoryBase<StandConfig, Guid>
{
    public Task<Result<StandConfig>> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Insert(StandConfig entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<StandConfig[]>> GetAll()
    {
        throw new NotImplementedException();
    }
}