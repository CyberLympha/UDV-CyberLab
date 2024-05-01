using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ILabEntryPointService
{
    public Task<Result> InsertAll(IReadOnlyList<LabEntryPoint> labEntryPoints);
}