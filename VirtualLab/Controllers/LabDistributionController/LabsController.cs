using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using ProxmoxApi;
using VirtualLab.Application.Interfaces;
using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.ValueObjects;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Controllers.LabDistributionController;

[ApiController]
[Route("[Controller]")]
public class LabsController : ControllerBase
{
    private readonly ILabCreationService _labCreationService;
    private readonly ILabManager _labManager;
    private readonly IUserLabProvider _userLabProvider;
    private readonly Guid UserId = Guid.NewGuid();

    public LabsController(
        IUserLabProvider userLabProvider,
        ILabCreationService labCreationService,
        ILabManager labManager)
    {
        _userLabProvider = userLabProvider;
        _labCreationService = labCreationService;
        _labManager = labManager;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] LabCreateRequest request)
    {
        var labConstructorData = CreateLabDto.From(request);
        var result = await _labCreationService.Create(labConstructorData);
        if (result.IsFailed) return BadRequest();


        return Ok();
    }

    [HttpGet] // ограничение на роли
    public async Task<ActionResult<IReadOnlyCollection<UserLabInfo>>> Get()
    {
        var user = new User();
        var labs = await _userLabProvider.GetInfoAll(user);

        return labs.Match(
            v => Ok(v),
            e => NotFound(e));
    }

    [HttpGet("{labId:guid}/start")]
    public async Task<ActionResult<ReadOnlyCollection<Credential>>> Start(Guid labId)
    {
        var createLab = await _labManager.StartNew(labId, UserId);

        return createLab.Match(
            s => Ok(s),
            e => BadRequest(e));
    }

    [HttpGet("{labId:guid}/end")]
    public async Task<ActionResult> End(Guid labId)
    {
        var removeLab = await _labManager.End(labId, UserId);

        return removeLab.Match(Ok, BadRequest);
    }
}