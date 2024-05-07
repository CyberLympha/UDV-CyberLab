using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface IVirtualMachineService
{
    public Task<Result> AddVm(VirtualMachine virtualMachine);
    public Task<Result> AddCredential(Credential credential);
    public Task<Result> GetAllByLabId(UserLab lab);
}