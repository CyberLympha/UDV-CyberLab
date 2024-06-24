using System.Text.RegularExpressions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using ProxmoxApi;
using VirtualLab.Application.Interfaces;
using VirtualLab.Controllers.TemplateController.Dto;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Controllers.TemplateController;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplatesController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpPost("add")]
    public async Task<ActionResult> Add([FromBody] TemplateAddRequest request)
    {
        var template = TemplateVm.From(request);

        var result = await _templateService.Add(template);

        return result.Match(
            Ok,
            BadRequest);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TemplateVmInfo>>> GetAll()
    {
        var templatesVm = await _templateService.GetAll();

        return templatesVm.Match(
            data => Ok(data),
            errors => BadRequest(errors));
    }
}