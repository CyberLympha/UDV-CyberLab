using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Controllers.TemplateController.Dto;
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

    public async Task<Result<IReadOnlyCollection<TemplateVmInfo>>> GetAll()
    {
        //todo: проверки

        var result = await _templateVms.GetAll();
        if (!result.TryGetValue(out var templates, out var errors))
        {
            return Result.Fail(errors);
        }

        var templatesInfos = new List<TemplateVmInfo>();
        foreach (var template in templates)
        {
            templatesInfos.Add(new TemplateVmInfo()
            {
                PveTemplateId = template.PveTemplateId
            });
        }
        
        
        return templatesInfos;
    }
}