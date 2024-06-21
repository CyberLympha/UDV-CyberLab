using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.Extensions;

namespace VirtualLab.Application;

public class TemplateService : ITemplateService
{
    private readonly ITemplateVmRepository _templateVms;

    public TemplateService(ITemplateVmRepository templateVms)
    {
        _templateVms = templateVms;
    }

    public async Task<Result> Add(TemplateVm templateVm)
    {
        //todo: проверки

        var result = await _templateVms.Insert(templateVm);
        if (result.IsFailedWithErrors(out var errors))
        {
            return Result.Fail(errors);
        }


        return Result.Ok();
    }
}