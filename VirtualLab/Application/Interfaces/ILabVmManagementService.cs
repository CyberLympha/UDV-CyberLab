using FluentResults;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Application.Interfaces;

public interface ILabVmManagementService
{

    public Task<Result<LabEntryPoint>> CreateVmWithLab(CloneVmTemplate cloneVmTemplate, string node);  // а может отдельный интерфейс под это?
    public Task<Result<LabEntryPoint>> CreateLab(LabNodeConfig labNodeConfig);
    
}