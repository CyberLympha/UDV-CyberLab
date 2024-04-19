using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repository;

public class LabRepository : RepositoryBase<Lab, Guid>, ILabRepository
{
    public LabRepository(FakeDbContext dbContext) : base(dbContext)
    {
    }
} 