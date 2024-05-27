using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class UserLabsRepository : RepositoryBase<UserLab, Guid> ,IUserLabRepository 
{
    public UserLabsRepository(LabDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<UserLab[]>> GetAllByUserId(Guid userId)
    {
        var userLabs = await _dbContext.Set<UserLab>()
            .Where(ul => ul.UserId == userId)
            .ToArrayAsync();

        return Result.Ok(userLabs);
    }

    //TODO: добавить фильтрацию по завершенным лабам
    public async Task<Result<UserLab[]>> GetAllCompletedByLabId(Guid labId)
    {
        var userLabs = await _dbContext.Set<UserLab>()
            .Where(ul => ul.LabId == labId)
            .ToArrayAsync();
        return Result.Ok(userLabs);
    }

    public async Task<Result<UserLab>> UpdateRate(Guid userLabId, int newRate)
    {
        var userLabResult = await Get(userLabId);
        if (userLabResult.IsFailed)
            return Result.Fail(userLabResult.Errors);

        var userLab = userLabResult.Value;

        userLab.Rate = newRate;
        await _dbContext.SaveChangesAsync();

        var updatedEntityEntry = _dbContext.Entry(userLab);
        var updatedEntity = updatedEntityEntry.Entity;
        return Result.Ok(updatedEntity);
    }
}