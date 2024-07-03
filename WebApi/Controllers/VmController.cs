using Corsinvest.ProxmoxVE.Api.Shared.Models.Node;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Vm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.VirtualMachineModels.Requests;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/vm")]
    [ApiController]
    public class VmController : ControllerBase
    {
        private readonly ILogger<VmController> _logger;
        private readonly IConfiguration _configuration;
        private readonly VmService _vmService;
        private readonly LabsService _labsService;

        public VmController(ILogger<VmController> logger, IConfiguration configuration, VmService vmService,
            LabsService labsService)
        {
            _configuration = configuration;
            _logger = logger;
            _vmService = vmService;
            _labsService = labsService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("start")]
        public async Task<ActionResult<NodeTask>> StartVm(string vmid)
        {
            try
            {
                var result = await _vmService.StartVm(vmid);
                return result as NodeTask;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("stop")]
        public async Task<object> StopVm(int vmid)
        {
            try
            {
                var result = await _vmService.StopVm(vmid);
                return result["data"];
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("ip")]
        [Produces(typeof(VmQemuAgentNetworkGetInterfaces))]
        public async Task<object> GetIpAddress(string vmid)
        {
            try
            {
                var result = await _vmService.GetVmIp(vmid);
                return result["data"];
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("task")]
        public async Task<ActionResult<object>> GetStatusTask(string uuid)
        {
            try
            {
                var result = await _vmService.GetTask(uuid);
                return result["data"];
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }


        [Authorize(Roles = "User")]
        [HttpPost("setPassword")]
        public async Task<ActionResult> SetPassword(ChangeCredentialsRequest credentials)
        {
            var result = await _vmService.SetPassword(credentials.Vmid, credentials.Username, credentials.Password,
                credentials.SshKey);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "User")]
        [HttpGet("config")]
        public async Task<ActionResult<object>> GetQemuConfig(string vmid)
        {
            try
            {
                var result = await _vmService.GetVmConfig(vmid);
                return result["data"];
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
        /// <summary>
        /// Retrieves a vmId in proxmox by id of vm record.
        /// </summary>
        /// <param name="id">The ID vm record.</param>
        /// <returns>An action result containing the retrieved vmId in proxmox.</returns>
        [HttpGet("get-vmId/{id}", Name = nameof(GetVmId))]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> GetVmId([FromRoute]string id)
        {
            try
            {
                return await _vmService.GetVmId(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}