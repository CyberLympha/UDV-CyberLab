using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface ITemplateVmRepository : IRepositoryBase<TemplateVm, Guid>
{
    public Task<Result<TemplateVm>> GetByTemplatePveVmId(int templateVmId);
}