using FluentResults;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Application.Interfaces;

public interface ILabConfigureGenerate
{
   
    public Task<Result<LabNodeConfig>> GenerateLabConfig(Guid labId);
}