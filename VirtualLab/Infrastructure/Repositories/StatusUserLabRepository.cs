using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Enums;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class StatusUserLabRepository : RepositoryBase<StatusUserLab, Guid>, IStatusUserLabRepository
{
    public StatusUserLabRepository(LabDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<StatusUserLab>> Get(StatusUserLabEnum statusUserLabEnum)
    {
        var status = await _dbContext.Set<StatusUserLab>().FirstOrDefaultAsync(s => s.Name == statusUserLabEnum);
        if (status == null)
        {
            return Result.Fail(ApiResultError.WithDataBase.NotFound(nameof(statusUserLabEnum), "empty"));
        }
        
        return status;
    }
}