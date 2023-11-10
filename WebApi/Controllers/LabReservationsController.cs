using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class LabReservationsController : ControllerBase
    {
        private readonly LabReservationsService _labReservationsService;

        public LabReservationsController(LabReservationsService labReservationsService)
        {
            _labReservationsService = labReservationsService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult> Create(LabReservation labReservation, Lab reservationLab)
        {
            try
            {
                await _labReservationsService.CreateAsync(labReservation, reservationLab);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<LabReservation>> GetAll()
        {
            return await _labReservationsService.GetAllAsync();
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Update(LabReservation labReservation, Lab reservationLab, string userId)
        {
            try
            {
                await _labReservationsService.UpdateAsync(labReservation, reservationLab, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
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
