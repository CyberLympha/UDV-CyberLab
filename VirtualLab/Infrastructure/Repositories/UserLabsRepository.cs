using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class UserLabsRepository : RepositoryBase<UserLab, Guid>, IUserLabRepository
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

    public async Task<Result<UserLab>> Get(Guid userId, Guid labId)
    {
        var userLab = await _dbContext.Set<UserLab>().FirstOrDefaultAsync(l => l.LabId == labId && l.UserId == userId);
        if (userLab == null)
            return Result.Fail($"this user {userId} not has lab {labId}");

        return userLab;
    }
}