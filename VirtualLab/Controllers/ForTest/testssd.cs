using Microsoft.AspNetCore.Mvc;
using VirtualLab.Application.Interfaces;

[ApiController]
[Route("[controller]")]
public class FreeIdsController : ControllerBase
{
    private readonly IProxmoxResourceManager _resourceManager;

    public FreeIdsController(IProxmoxResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    [HttpGet("GetFreeIds")]
    public async Task<IActionResult> GetFreeIds()
    {
        try
        {
            var result = await _resourceManager.GetFreeVmbrs("pve", 3); // Замените "node_name" на имя узла, если необходимо
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}