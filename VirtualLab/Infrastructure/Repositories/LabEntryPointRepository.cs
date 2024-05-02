using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class LabEntryPointRepository : RepositoryBase<LabEntryPoint, Guid>,ILabEntryPointRepository
{
    public LabEntryPointRepository(FakeDbContext dbContext) : base(dbContext)
    {
    }
}