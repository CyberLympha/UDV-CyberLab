using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VirtualLab.Domain.Interfaces.Proxmox;

[ApiController]
[Route("[controller]")]
public class ProxmoxController : ControllerBase
{
    private readonly IProxmoxVm _proxmoxNetwork;

    public ProxmoxController(IProxmoxVm proxmoxNetwork)
    {
        _proxmoxNetwork = proxmoxNetwork;
    }

    [HttpGet("{node}/{qemu}")]
    public async Task<IActionResult> GetStatus(string node, int qemu)
    {
        try
        {
            var statusResult = await _proxmoxNetwork.GetStatus(node, qemu);

            if (statusResult.IsSuccess)
            {
                // Обработка успешного результата
                return Ok(statusResult.Value);
            }
            else
            {
                // Обработка ошибки
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            // Обработка исключений
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}