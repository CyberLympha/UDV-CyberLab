using FluentResults;
using MongoDB.Driver;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.MongoRepository;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Lib;

namespace VirtualLab.Infrastructure.Repositories;

public class ConfigStandRepository : RepositoryBaseMongo<StandConfig,Guid>, IConfigStandRepository
{
    public ConfigStandRepository(IMongoContext dbContext, IMongoContext db) : base(dbContext)
    {
    }

    public async Task<Result<StandConfig>> GetByLabId(Guid guid)
    {
        var getByLabId =  Builders<StandConfig>.Filter.Eq("LabId", guid);

        var data = await DbSet.FindAsync(getByLabId);

        
        return data.SingleOrDefault();
    }
}