using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using Guid = System.Guid;

namespace VirtualLab.Application.Interfaces;

public interface ILabVirtualMachineManager
{
    public Task<Result<IReadOnlyList<VirtualMachineInfo>>> CreateLab(LabConfig labConfig);

    public Task<Result> RemoveLab();
}