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
    public Task<Result<IReadOnlyList<Credential>>> CreateLab(LabConfig labConfig);
    public Task<Result<IReadOnlyList<Credential>>> GetCredentials(Guid labId); //todo: по id лабы достаём все возможные credendial.
}