
using Microsoft.AspNetCore.Mvc;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application;
using VirtualLab.Controllers.LabCreationService.Dto;

namespace VirtualLab.Controllers.LabCreationService;


[ApiController]
[Route("[controller]")] // todo: naming пока такой себе
public class LabCreationController : ControllerBase
{
    private readonly ILabCreationService _labCreationService;

    public LabCreationController(ILabCreationService labCreationService)
    {
        _labCreationService = labCreationService;
    }

    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] LabCreateRequest request) 
    {
        var lab = Lab.From(request);
        var result = await _labCreationService.Create(lab);
        if (result.IsFailed)
        {
            return BadRequest();
        }

        return Ok();
    }
}