using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.LabWorks;
using WebApi.Services;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing laboratory work operations.
/// </summary>
[Route("/api/lab-works")]
[ApiController]
public class LabWorkController : ControllerBase
{
    private readonly LabWorkService labWorkService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LabWorkController"/> class.
    /// </summary>
    /// <param name="labWorkService">The service responsible for laboratory work operations.</param>
    public LabWorkController(LabWorkService labWorkService)
    {
        this.labWorkService = labWorkService;
    }
    
    /// <summary>
    /// Creates a new laboratory work.
    /// </summary>
    /// <param name="creationRequest">The request containing information for creating the laboratory work.</param>
    /// <returns>An action result indicating the outcome of the create operation.</returns>
    [HttpPost("create", Name = nameof(Create))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<ActionResult> Create(CreateLabWorkRequest creationRequest)
    {
        try
        {
            var labWork = new LabWork()
            {
                VmId = creationRequest.VmId,
                Description = creationRequest.Description,
                Title = creationRequest.Title,
                ShortDescription = creationRequest.ShortDescription,
                InstructionId = creationRequest.InstructionId
            };
            await labWorkService.CreateAsync(labWork);
            return StatusCode(201);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Retrieves a laboratory work by its ID.
    /// </summary>
    /// <param name="id">The ID of the laboratory work to retrieve.</param>
    /// <returns>An action result containing the retrieved laboratory work.</returns>
    [HttpGet("get/{id}", Name = nameof(Get))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(LabWork), 200)]
    public async Task<ActionResult<LabWork>> Get([FromRoute]string id)
    {
        try
        {
            return await labWorkService.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all laboratory works.
    /// </summary>
    /// <returns>A list of all laboratory works.</returns>
    [HttpGet("get", Name = nameof(GetAll))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(List<LabWork>), 200)]
    public async Task<List<LabWork>> GetAll()
    {
        return await labWorkService.GetAllAsync();
    }
    
    /// <summary>
    /// Updates an existing laboratory work.
    /// </summary>
    /// <param name="updateRequest">The request containing information for updating the laboratory work.</param>
    /// <returns>An action result indicating the outcome of the update operation.</returns>
    [HttpPost("update", Name = nameof(Update))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Update(UpdateLabWorkRequest updateRequest)
    {
        try
        {
            var labWork = new LabWork()
            {
                VmId = updateRequest.VmId,
                Description = updateRequest.Description,
                Title = updateRequest.Title,
                ShortDescription = updateRequest.ShortDescription,
                InstructionId = updateRequest.InstructionId
            };
            await labWorkService.UpdateAsync(labWork);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Deletes a laboratory work by its ID.
    /// </summary>
    /// <param name="labWorkId">The ID of the laboratory work to delete.</param>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    [HttpPost("delete/{labWorkId}", Name = nameof(Delete))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete([FromRoute]string labWorkId)
    {
        try
        {
            await labWorkService.DeleteAsync(labWorkId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}