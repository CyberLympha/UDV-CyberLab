using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.LabWorks;
using WebApi.Services;

namespace WebApi.Controllers;

/// <summary>
///     Controller for managing lab work instruction operations.
/// </summary>
[Route("/api/lab-work-instruction")]
[ApiController]
public class LabWorkInstructionController : ControllerBase
{
    private readonly LabWorkInstructionService labWorkInstructionService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LabWorkInstructionController" /> class.
    /// </summary>
    /// <param name="labWorkInstructionService">The service responsible for lab work instruction operations.</param>
    public LabWorkInstructionController(LabWorkInstructionService labWorkInstructionService)
    {
        this.labWorkInstructionService = labWorkInstructionService;
    }

    /// <summary>
    ///     Creates a new lab work instruction.
    /// </summary>
    /// <param name="creationRequest">The request containing information for creating the lab work instruction.</param>
    /// <returns>An action result indicating the outcome of the create operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("create", Name = nameof(Create))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Create(CreateLabWorkInstructionRequest creationRequest)
    {
        try
        {
            var instructionStep = new LabWorkInstruction
            {
                Steps = creationRequest.Steps,
                LogFilePaths = creationRequest.LogFilePaths
            };
            await labWorkInstructionService.CreateAsync(instructionStep);
            return StatusCode(201);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Updates an existing lab work instruction.
    /// </summary>
    /// <param name="updateRequest">The request containing information for updating the lab work instruction.</param>
    /// <returns>An action result indicating the outcome of the update operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("update", Name = nameof(Update))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(UpdateLabWorkInstructionRequest updateRequest)
    {
        try
        {
            var instructionStep = new LabWorkInstruction
            {
                Id = updateRequest.Id,
                Steps = updateRequest.Steps,
                LogFilePaths = updateRequest.LogFilePaths
            };
            await labWorkInstructionService.UpdateAsync(instructionStep);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves a lab work instruction by its ID.
    /// </summary>
    /// <param name="id">The ID of the lab work instruction to retrieve.</param>
    /// <returns>An action result containing the retrieved lab work instruction.</returns>
    [HttpGet("get/{id}", Name = nameof(Get))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(LabWorkInstruction), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<LabWorkInstruction>> Get([FromRoute] string id)
    {
        try
        {
            return await labWorkInstructionService.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves all lab work instructions.
    /// </summary>
    /// <returns>A list of all lab work instructions.</returns>
    [HttpGet("get", Name = nameof(GetAll))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(List<LabWorkInstruction>), 200)]
    public async Task<List<LabWorkInstruction>> GetAll()
    {
        return await labWorkInstructionService.GetAllAsync();
    }

    /// <summary>
    ///     Deletes a lab work instruction by its ID.
    /// </summary>
    /// <param name="id">The ID of the lab work instruction to delete.</param>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("delete/{id}", Name = nameof(Delete))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        try
        {
            await labWorkInstructionService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}