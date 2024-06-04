using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VirtualLab.Domain.Interfaces.Proxmox;

[ApiController]
[Route("[controller]")]
public class VmController : ControllerBase
{
    private readonly IProxmoxNetwork _proxmoxVmService;

    public VmController(IProxmoxNetwork proxmoxVmService)
    {
        _proxmoxVmService = proxmoxVmService;
    }

    [HttpGet("networks/{node}/{vmId}")]
    public async Task<IActionResult> GetNetworks(string node, int vmId)
    {
        try
        {
            var networksResult = await _proxmoxVmService.GetAllNetworksBridgeByVm(vmId, node);
            if (networksResult.IsSuccess)
            {
                return Ok(networksResult.Value);
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