using FluentResults;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox.Requests;

namespace VirtualLab.Application.Interfaces;


//todo: подумать над всей этой реализацией
public interface IVmService
{
    // 1
    public Task<Result> Clone(CloneVmConfig vmConfig, string node);
    
    // 4 поставить другой интерфейс
    public Task<Result> UpdateDeviceInterface(UpdateInterfaceForVm request);
    
    // 5 запуск машни
    public Task<Result> StartVm(LaunchVm request);
    public Task<Result<Ip>> GetIp(string node, int qemu); // получим ли мы только ip, пока хз
}

