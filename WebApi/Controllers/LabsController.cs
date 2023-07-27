using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Node;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Vm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/labs")]
    [ApiController]
    public class LabsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LabsService labsService;

        public LabsController(IConfiguration configuration, LabsService labsService)
        {
            _configuration = configuration;
            this.labsService = labsService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<List<Lab>>> GetLabs()
        {
            return await labsService.GetAllLabs();
        }
        
        // [Authorize(Roles = "User,Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<string>> CreateVm(CreateLabRequest request)
        {
            try
            {
                return await labsService.CreateLab(request.Id);
            }
            catch (Exception e)
            {
                return BadRequest("Не смогли создать машину");
            }
        }
        
        [HttpGet("status")]
        [Produces(typeof(List<VmQemuStatusCurrent>))]
        public async Task<ActionResult<List<object>>> GetLabStatus(string id)
        {
            try
            {
                return await labsService.GetClusterStatus(id);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetUserLabs(string id)
        {
            return await labsService.GetLabUser(id);
        }
    }
}