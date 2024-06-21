using FluentResults;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Repositories;

namespace VirtualLab.Domain.Interfaces.MongoRepository;

public interface IConfigStandRepository : IRepositoryBase<StandConfig, Guid>
{
    public Task<Result<StandConfig>> GetByLabId(Guid guid);
}