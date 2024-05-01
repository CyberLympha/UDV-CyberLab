using FluentResults;
using VirtualLab.Domain.Entities;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Application.Interfaces;

public interface ILabManager
{
    public Task<Result<IReadOnlyList<LabEntryPoint>>> StartNew(System.Guid labId);
    public Task<Result<string>> GetStatus(Guid labId);
}