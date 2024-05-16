using System.Collections.Immutable;
using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface IVirtualMachineDataHandler
{
    public Task<Result> AddVm(VirtualMachine virtualMachine);
    public Task<Result> AddCredential(Credential credential);
    public Task<Result<ImmutableArray<VirtualMachine>>> GetAllByUserLabId(Guid userLabId);
    public Task<Result> DeleteAllByUserLabId(Guid userLabId);
}