using System.Collections.ObjectModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProxmoxApi;
using VirtualLab.Application.Interfaces;
using VirtualLab.Controllers.LabCreationService.Dto;
using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Controllers.LabDistributionController;

[ApiController]
[Route("[Controller]")]
public class LabsController : ControllerBase
{
    private readonly IUserLabProvider _userLabProvider;
    private readonly ILabProvider _labProvider;
    private readonly ILabCreationService _labCreationService;
    private readonly ILabManager _labManager;
    private readonly Guid UserId = Guid.NewGuid();

    public LabsController(
        IUserLabProvider userLabProvider,
        ILabCreationService labCreationService,
        ILabManager labManager,
        ILabProvider labProvider)
    {
        _userLabProvider = userLabProvider;
        _labCreationService = labCreationService;
        _labManager = labManager;
        _labProvider = labProvider;
    }

    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] LabCreateRequest request)
    {
        var lab = Lab.From(request);
        var result = await _labCreationService.Create(lab);
        if (result.IsFailed) return BadRequest();


        return Ok();
    }

    [HttpGet] // ограничение на роли
    [Authorize(Policy = "Student")]
    public async Task<ActionResult<IReadOnlyCollection<UserLabInfo>>> Get()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        var user = new User { Id = Guid.Parse(id) };
        var labs = await _userLabProvider.GetInfoAll(user);
        
        return labs.Match(
            v => Ok(v),
            e => NotFound(e));
    }

    [HttpGet("teacher/{teacherId:guid}")]
    [Authorize(Policy = "Teacher")] //todo: брать id из claims
    public async Task<ActionResult<IReadOnlyCollection<TeacherLabShortInfo>>> GetTeacherLabs(Guid teacherId)
    {
        var labs = await _labProvider.GetAllByUserId(teacherId);
        
        return labs.Match(
            l => Ok(l),
            e => NotFound(e));
    }

    [HttpGet("{labId:guid}/userLabs")]
    [Authorize(Policy = "Teacher")]
    public async Task<ActionResult<IReadOnlyCollection<AttemptShortInfo>>> GetAttemptsPerLab(Guid labId)
    {
        var attempts = await _userLabProvider.GetAllCompletedByLabId(labId);

        return attempts.Match(
            a => Ok(a),
            e => NotFound(e));
    }

    [HttpGet("attempts/{userLabId:guid}")]
    [Authorize(Policy = "Teacher")]
    public async Task<ActionResult<AttemptFullInfo>> GetAttempt(Guid userLabId)
    {
        var attemptResult = await _userLabProvider.GetAttempt(userLabId);

        return attemptResult.Match(
            a => Ok(a),
            e => NotFound(e));
    }

    [HttpPatch("attempts/{userLabId:guid}")]
    [Authorize(Policy = "Teacher")]
    public async Task<ActionResult<AttemptFullInfo>> UpdateUserLabRate(Guid userLabId, [FromBody]RateUpdateRequest request)
    {
        var attemptResult = await _userLabProvider.UpdateUserLabRate(userLabId, request.NewRate);

        return attemptResult.Match(
            a => Ok(a),
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