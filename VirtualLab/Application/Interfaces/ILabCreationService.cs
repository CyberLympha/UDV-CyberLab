using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ILabCreationService
{
    public Task<Result> Create(Lab guid);
    public Task<Result> Change(Lab guid);
    public Task<Result<Lab>> Get(Lab guid);
}