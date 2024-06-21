using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class LabEntryPointRepository : RepositoryBase<Credential, Guid>, ILabEntryPointRepository
{
    public LabEntryPointRepository(LabDbContext dbContext) : base(dbContext)
    {
    }
}