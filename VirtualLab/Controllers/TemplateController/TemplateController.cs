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
public class TemplateController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpPost("/[action]")]
    public async Task<ActionResult> Add([FromBody] TemplateAddRequest request)
    {
        var template = TemplateVm.From(request);

        var result = await _templateService.Add(template);

        return result.Match(
            Ok,
            BadRequest);
    }
}