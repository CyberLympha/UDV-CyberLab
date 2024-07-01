using Microsoft.AspNetCore.Mvc;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Infrastructure.Pve;

[ApiController]
[Route("api/[controller]")]
public class ProxmoxController : ControllerBase
{
    private readonly IProxmoxNode _pveNode;

    public ProxmoxController(IProxmoxNode pveNode)
    {
        _pveNode = pveNode;
    }

    [HttpGet("interfaces/{node}")]
    public async Task<IActionResult> GetInterfaces(string node)
    {
        try
        {
            var result = await _pveNode.GetAllIFaceId(node);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            // Обработка исключений
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
