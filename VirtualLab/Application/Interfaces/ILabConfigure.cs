using FluentResults;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;

namespace VirtualLab.Application.Interfaces;

public interface ILabConfigure
{
    public Task<Result<StandCreateConfig>> GetConfigByLab(Guid labId);


    Task<Result<StandRemoveConfig>> GetConfigByUserLab(Guid userLabId);
}