using Corsinvest.ProxmoxVE.Api;
using Microsoft.AspNetCore.Mvc;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Infrastructure;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxmoxController : ControllerBase
    {
        private readonly PveClient _client;
        private readonly ILog _log;
        private IProxmoxVm _proxmoxVm;

        public ProxmoxController(PveClient client, ILog log, ProxmoxAuthData proxmoxData, IProxmoxVm proxmoxVm)
        {
            _client = client;
            _log = log;
            _proxmoxVm = proxmoxVm;
        }

        [HttpGet("{node}/qemu")]
        public async Task<IActionResult> GetAllQemu([FromRoute] string node)
        {
            try
            {
                var result = await _proxmoxVm.GetAllQemu("pve");
                if (result.IsFailed)
                {
                    return BadRequest(new { message = "API error occurred" });
                }
                
                
                return Ok();
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, $"Method '{ex.Message}' not implemented.");
            }
        }
    }
}
