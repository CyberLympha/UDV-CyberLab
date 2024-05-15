using System.Collections.Immutable;
using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IVirtualMachineRepository : IRepositoryBase<VirtualMachine, Guid>
{
    public Task<Result<ImmutableArray<VirtualMachine>>> GetAllByUserLab(Guid userLabId);
    public Task<Result> DeleteByUserLabId(Guid userLabId);
}