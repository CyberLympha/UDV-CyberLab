using System.Configuration;
using System.Net;
using Corsinvest.ProxmoxVE.Api;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Vm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;
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

        public VmController(ILogger<VmController> logger, IConfiguration configuration, VmService vmService)
        {
            _configuration = configuration;
            _logger = logger;
            _vmService = vmService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("get")]
        public async Task<object> GetVms()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var rand = new Random();
            var response = await _vmService.GetVmsAsync(rand.Next(150, 999999), "gggweSSfsdfsdgsdw");
            if (response.ResponseInError)
            {
                return BadRequest(response.GetError());
            }

            return response.Response;
        }

        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<object> CreateVm(Vm vm)
        {
            var result = await _vmService.CreateVmAsync(vm);
            if (result.ResponseInError)
            {
                return BadRequest("Не смогли создать машину");
            }

            return StatusCode(201);
        }

        [Authorize(Roles = "User")]
        [HttpGet("start")]
        public async Task<object> StartVm(int vmid)
        {
            var result = await _vmService.StartVm(vmid);
            if (result == null)
            {
                return BadRequest();
            }

            return result.Response;
        }

        [Authorize(Roles = "User")]
        [HttpGet("stop")]
        public async Task<object> StopVm(int vmid)
        {
            var result = await _vmService.StopVm(vmid);
            if (result == null)
            {
                return BadRequest();
            }

            return result.Response;
        }

        [Authorize(Roles = "User")]
        [HttpGet("status")]
        [Produces(typeof(VmBaseStatusCurrent))]
        public async Task<ActionResult<object>> GetStatus(int vmid)
        {
            var result = await _vmService.GetStatus(vmid);
            if (result != null)
            {
                return result["data"];
            }

            return BadRequest();
        }

        [Authorize(Roles = "User")]
        [HttpPost("setPassword")]
        public async Task<ActionResult> SetPassword(ChangeCredentialsRequest creds)
        {
            var result = await _vmService.SetPassword(creds.Vmid, creds.Username, creds.Password, creds.SshKey);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}