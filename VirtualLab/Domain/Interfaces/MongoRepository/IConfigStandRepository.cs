using FluentResults;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Repositories;

namespace VirtualLab.Domain.Interfaces.MongoRepository;

public interface IConfigStandRepository: IRepositoryBase<StandConfig, Guid>
{
    public Task<Result<StandConfig>>  GetByLabId(Guid guid);
}