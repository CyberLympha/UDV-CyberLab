using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using Guid = System.Guid;

namespace VirtualLab.Application.Interfaces;

public interface ILabVirtualMachineManager
{
    public Task<Result<IReadOnlyList<LabEntryPoint>>> CreateLab(LabConfig labConfig);
    public Task<Result<IReadOnlyList<LabEntryPoint>>> GetEntryPoint(Guid labId);
}