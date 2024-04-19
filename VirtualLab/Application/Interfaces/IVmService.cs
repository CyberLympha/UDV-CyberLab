using FluentResults;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Application.Interfaces;


//todo: подумать над всей этой реализацией
public interface IVmService
{
    // 1
    public Task<Result> Clone(CloneVmTemplate vmTemplate, string node);
    
    // 4 поставить другой интерфейс
    public Task<Result> ChangeDeviceInterface(ChangeInterfaceForVm request);
    
    // 5 запуск машни
    public Task<Result> StartVm(LaunchVm request);
    public Task<Result> GetIp(long id); // получим ли мы только ip, пока хз
}

