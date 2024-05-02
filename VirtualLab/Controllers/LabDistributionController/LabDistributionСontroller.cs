using Microsoft.AspNetCore.Mvc;
using ProxmoxApi;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Controllers.LabCreationService.Dto;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Controllers.LabDistributionController;

[ApiController]
[Route("[Controller]")]
public class LabsController : ControllerBase
{
    private readonly IUserLabProvider _userLabProvider;
    private readonly ILabCreationService _labCreationService;
    private readonly ILabManager _labManager;

    public LabsController(
        IUserLabProvider userLabProvider,
        ILabCreationService labCreationService, 
        ILabManager labManager)
    {
        _userLabProvider = userLabProvider;
        _labCreationService = labCreationService;
        _labManager = labManager;
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
    public async Task<ActionResult<IReadOnlyCollection<UserLabInfo>>> Get()
    {
        var list = new List<int>();
        var d = list[55];
        
        var user = new User();
        var labs = await _userLabProvider.GetInfoAll(user);
        
        return labs.Match(
            v => Ok(v),
            e => NotFound(e));
    }


    // todo: метод всё более жирный; нужно придумать еще один класс, который будет логично это в себе инкапсулировать.
    [HttpGet("{labId:guid}/start")] //todo: очень важно реализовать проверку, а есть ли эта лаба у юзера. сейчас лаба создаётся по LabId, а не по userLabId, что даёт возможность создавать бесконечно лаб. для одного пользователя))
    public async Task<ActionResult<IReadOnlyList<LabEntryPoint>>> Start(System.Guid labId)
    {
        var createLab = await _labManager.StartNew(labId);
            
        return createLab.Match(
            s => Ok(s),
            e => BadRequest(e));
    }
}