using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using Guid = System.Guid;

namespace VirtualLab.Application.Interfaces;

public interface IStandManager
{
    public Task<Result<IReadOnlyList<VirtualMachineInfo>>> CreateStand(StandCreateConfig standCreateConfig);

    public Task<Result> RemoveStand(StandRemoveConfig standRemoveConfig);
}