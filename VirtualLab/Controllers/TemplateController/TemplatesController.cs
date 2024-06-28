using Microsoft.AspNetCore.Mvc;
using ProxmoxApi;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Controllers.TemplateController;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly IPveTemplateService _pveTemplateService;

    public TemplatesController(IPveTemplateService pveTemplateService)
    {
        _pveTemplateService = pveTemplateService;
    }

    [HttpGet("{node}/{id:int}")]
    public async Task<ActionResult<TemplateData>> Get(int id, string node) // нужно сделать dto
    {
        var template = await _pveTemplateService.GetDataTemplate(id, node);

        return template.Match(
            data => Ok(data),
            error => BadRequest(error));
    }
}