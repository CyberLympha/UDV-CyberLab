using FluentResults;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;

namespace VirtualLab.Application.Interfaces;

public interface IStandManager
{
    public Task<Result<IReadOnlyList<VirtualMachineInfo>>> Create(StandCreateConfig standCreateConfig);

    public Task<Result> Delete(StandRemoveConfig standRemoveConfig);
}