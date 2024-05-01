using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;
using Guid = System.Guid;

namespace VirtualLab.Infrastructure.Repositories;

public class LabEntryPointRepositoryRepository : RepositoryBase<LabEntryPoint, Guid>, ILabEntryPointRepository
{
    public LabEntryPointRepositoryRepository(FakeDbContext dbContext) : base(dbContext)
    {
    }
}