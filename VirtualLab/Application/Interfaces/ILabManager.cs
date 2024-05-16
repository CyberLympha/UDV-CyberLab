using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ILabManager
{
    public Task<Result<ReadOnlyCollection<Credential>>> StartNew(Guid labId, Guid userId);
    Task<Result> End(Guid labId, Guid newGuid);
}