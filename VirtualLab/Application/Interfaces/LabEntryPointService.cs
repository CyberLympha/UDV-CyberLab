using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.Repositories;

namespace VirtualLab.Application.Interfaces;

public class LabEntryPointService : ILabEntryPointService
{
    private readonly LabEntryPointRepositoryRepository entryPoints;

    public LabEntryPointService(LabEntryPointRepositoryRepository entryPoints)
    {
        this.entryPoints = entryPoints;
    }

    public Task<Result> InsertAll(IReadOnlyList<LabEntryPoint> labEntryPoints)
    {
        throw new NotImplementedException();
    }
}