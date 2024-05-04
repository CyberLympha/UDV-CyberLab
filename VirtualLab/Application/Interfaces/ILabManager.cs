using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ILabManager
{
    public Task<Result<IReadOnlyList<Credential>>> StartNew(Guid labId);
    public Task<Result<string>> GetStatus(Guid labId);
}