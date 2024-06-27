using FluentResults;
using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class TemplateVmRepository : RepositoryBase<TemplateVm, Guid>
{
    public TemplateVmRepository(LabDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<TemplateVm>> GetByTemplatePveVmId(int templateVmId)
    {
        var templateVm = await _dbContext.Set<TemplateVm>().FirstOrDefaultAsync(e => e.PveTemplateId == templateVmId);
        if (templateVm == null)
            return Result.Fail(ApiResultError.WithDataBase.NotFound(nameof(TemplateVm), templateVmId.ToString()));

        return templateVm;
    }
}