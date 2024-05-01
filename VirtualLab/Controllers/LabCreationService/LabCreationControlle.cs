
using Microsoft.AspNetCore.Mvc;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
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

    
}