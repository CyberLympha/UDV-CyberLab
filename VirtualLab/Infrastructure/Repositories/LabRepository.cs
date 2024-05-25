using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class LabRepository : RepositoryBase<Lab, Guid>, ILabRepository
{
    public LabRepository(LabDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<Result<Lab[]>> GetAllByCreatorId(Guid creatorId)
    {
        //TODO: проверка на существование creator
        var labs = await _dbContext.Set<Lab>()
            .Where(l => l.CreatedBy == creatorId)
            .ToArrayAsync();

        return Result.Ok(labs);
    }
} 