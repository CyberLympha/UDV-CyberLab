using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface IVirtualMachineService
{
    public Task AddVm(VirtualMachine virtualMachine);
    public Task AddCredential(Credential credential);
    public Task GetAllByLabId(UserLab lab);
}