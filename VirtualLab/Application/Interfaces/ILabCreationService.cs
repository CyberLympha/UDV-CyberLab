using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Application.Interfaces;

public interface ILabCreationService
{
    public Task<Result> Create(Lab guid, StandConfig stand);
    public Task<Result> Change(Lab guid);
    public Task<Result<Lab>> Get(Lab guid);
}