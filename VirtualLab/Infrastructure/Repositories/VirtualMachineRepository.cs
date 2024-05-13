using System.Collections.Immutable;
using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class VirtualMachineRepository : RepositoryBase<VirtualMachine, Guid>,IVirtualMachineRepository
{
    public VirtualMachineRepository(FakeDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Result<ImmutableArray<VirtualMachine>>> GetAllByUserLab(Guid userLab)
    {
        throw new NotImplementedException();
    }
}