using Microsoft.AspNetCore.Mvc;
using ProxmoxApi;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Controllers.LabDistributionController;

[ApiController]
[Route("[Controller]")]
public class LabsDistributionController : ControllerBase
{
    private readonly ILabProvider _labProvider;
    private readonly ILabVmManagementService _labVmManagementService;
    private readonly ILabConfigureGenerate _labConfigureGenerate;

    public LabsDistributionController(ILabProvider labProvider,
        ILabVmManagementService labVmManagementService,
        ILabConfigureGenerate labConfigureGenerate)
    {
        _labProvider = labProvider;
        _labVmManagementService = labVmManagementService;
        _labConfigureGenerate = labConfigureGenerate;
    }

    [HttpGet("my/labs")] // ограничение на роли
    public async Task<ActionResult<IReadOnlyCollection<UserLabsInfo>>> Get()
    {
        var user = new User();
        var labs = await _labProvider.GetAllByUser(user);

        return labs.Match(
            v => Ok(v),
            e => NotFound(e));
    }

    [HttpGet("start/{labId:guid}")]
    public async Task<ActionResult<LabEntryPoint>> CreateLab(Guid labId)
    {
        var configLab = await _labConfigureGenerate.GenerateLabConfig(labId);
        if (configLab.IsFailed) return BadRequest(configLab.Reasons);
        
        var labEntryPoint = await _labVmManagementService.CreateLab(configLab.Value);

        return labEntryPoint.Match(
            s => Ok(s),
            e => BadRequest(e));
    }
}