using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.Repositories;

namespace VirtualLab.Application;

public class LabEntryPointService : ILabEntryPointService
{
    private readonly ILabEntryPointRepository entryPoints;

    public LabEntryPointService(ILabEntryPointRepository entryPoints)
    {
        this.entryPoints = entryPoints;
    }

    public Task<Result> InsertAll(IReadOnlyList<LabEntryPoint> labEntryPoints)
    {
        throw new NotImplementedException();
    }
}