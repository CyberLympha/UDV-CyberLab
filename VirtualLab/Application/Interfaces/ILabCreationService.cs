using FluentResults;
using VirtualLab.Domain.Entities;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Application.Interfaces;

public interface ILabCreationService
{
    public Task<Result> Create(Guid guid);
    public Task<Result> Change(Guid guid);
    public Task<Result<Guid>> Get(Guid guid);
}