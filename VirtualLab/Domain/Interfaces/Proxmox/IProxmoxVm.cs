using FluentResults;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;

namespace VirtualLab.Domain.Interfaces.Proxmox;


public interface IProxmoxVm
{
    // 1
    public Task<Result> Clone(CloneVmConfig vmConfig, string node);
    
    // 4 поставить другой интерфейс
    public Task<Result> UpdateDeviceInterface(UpdateInterfaceForVm request);


    public Task<Result<ProxmoxVmStatus>> GetStatus(string node, int qemu);
    
    // 5 запуск машни
    public Task<Result> StartVm(LaunchVm request);

    public Task<Result> StopVm(string node, int qemu);
    public Task<Result<Ip>> GetIp(string node, int qemu); // получим ли мы только ip, пока хз
}

