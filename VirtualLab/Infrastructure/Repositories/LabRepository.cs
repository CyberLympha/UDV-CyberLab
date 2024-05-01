using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Infrastructure.Repository;

public class LabRepository : RepositoryBase<Guid, System.Guid>, ILabRepository
{
    public LabRepository(FakeDbContext dbContext) : base(dbContext)
    {
    }
} 