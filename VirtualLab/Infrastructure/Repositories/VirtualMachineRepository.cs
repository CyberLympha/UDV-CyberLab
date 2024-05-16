using System.Collections.Immutable;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class VirtualMachineRepository : RepositoryBase<VirtualMachine, Guid>, IVirtualMachineRepository
{
    private DbContext _db;

    public VirtualMachineRepository(LabDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public async Task<Result<ImmutableArray<VirtualMachine>>> GetAllByUserLab(Guid userLabId)
    {
        return (await _db.Set<VirtualMachine>()
                .Where(x => x.UserLabId == userLabId)
                .ToArrayAsync())
            .ToImmutableArray();
    }

    public async Task<Result> DeleteByUserLabId(Guid userLabId)
    {
        var result = _db.Set<VirtualMachine>()
            .Where(x => x.UserLabId == userLabId)
            .Select(machine => _db.Set<VirtualMachine>().Remove(machine));

        await _db.SaveChangesAsync();

        return Result.Ok();
    }
}