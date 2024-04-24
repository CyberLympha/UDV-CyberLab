using FluentResults;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Application.Interfaces;

public interface ILabVmManagementService
{
    public Task<Result<LabEntryPoint>> CreateLab(LabCreateRequest labCreateRequest);
    
}