using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.MongoRepository;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Lib;

namespace VirtualLab.Infrastructure.Repositories;

public class ConfigRepository : RepositoryBaseMongo<StandConfig,Guid>, IConfigStandRepository
{
    public ConfigRepository(IMongoContext dbContext) : base(dbContext)
    {
    }
    
        
}