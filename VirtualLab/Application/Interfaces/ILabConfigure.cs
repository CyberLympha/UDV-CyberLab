using FluentResults;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;

namespace VirtualLab.Application.Interfaces;

public interface ILabConfigure
{
    public Task<Result<LabConfig>> GetTemplateConfig(Guid labId);
    public Task<Result<LabConfig>> GenerateLabConfig(LabConfig labConfig);
    
    
}