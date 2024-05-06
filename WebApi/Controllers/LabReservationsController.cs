using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using WebApi.Model.LabReservationModels;
using WebApi.Model.LabReservationModels.Requests;

namespace WebApi.Controllers
{
    [Route("/api/schedule")]
    [ApiController]
    public class LabReservationsController : ControllerBase
    {
        private readonly LabReservationsService _labReservationsService;
        private readonly UserService _userService;

        public LabReservationsController(LabReservationsService labReservationsService, UserService userService)
        {
            _labReservationsService = labReservationsService;
            _userService = userService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult> Create(CreateLabReservationRequest creationRequest)
        {
            try
            {
                var labReservation = new LabReservation()
                {
                    Description = creationRequest.Description,
                    Theme = creationRequest.Theme,
                    Lab = creationRequest.Lab,
                    Reservor = await _userService.GetAsyncById(creationRequest.ReservorId),
                    TimeEnd = new DateTime(creationRequest.TimeEnd),
                    TimeStart = new DateTime(creationRequest.TimeStart),
                };
                await _labReservationsService.CreateAsync(labReservation);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<LabReservation>> Get(string id)
        {
            try
            {
                return await _labReservationsService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get")]
        public async Task<List<LabReservation>> GetAll()
        {
            return await _labReservationsService.GetAllAsync();
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Update(UpdateLabReservationRequest updateRequest)
        {
            try
            {
                var labReservation = new LabReservation()
                {
                    Id = updateRequest.Id,
                    Description = updateRequest.Description,
                    Theme = updateRequest.Theme,
                    Lab = updateRequest.Lab,
                    Reservor = await _userService.GetAsyncById(updateRequest.ReservorId),
                    TimeEnd = new DateTime(updateRequest.TimeEnd),
                    TimeStart = new DateTime(updateRequest.TimeStart),
                };
                await _labReservationsService.UpdateAsync(labReservation, updateRequest.CurrentUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete/{labReservationId}/{userId}")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(string labReservationId, string userId)
        {
            try
            {
                await _labReservationsService.DeleteAsync(labReservationId, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
