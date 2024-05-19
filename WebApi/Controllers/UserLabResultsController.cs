using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.LabWorks;
using WebApi.Services;

namespace WebApi.Controllers;

/// <summary>
/// API controller for managing user lab results.
/// </summary>
[Route("/api/user-lab-result")]
[ApiController]
public class UserLabResultsController : ControllerBase
{
    private readonly UserLabResultsService userLabResultsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLabResultsController"/> class.
    /// </summary>
    /// <param name="userLabResultsService">The service to manage user lab results.</param>
    public UserLabResultsController(UserLabResultsService userLabResultsService)
    {
        this.userLabResultsService = userLabResultsService;
    }

    /// <summary>
    /// Creates a new user lab result.
    /// </summary>
    /// <param name="creationRequest">The request object containing details of the user lab result to be created.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    /// <response code="201">Indicates that the user lab result was successfully created.</response>
    /// <response code="500">Indicates that an internal server error occurred.</response>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("create", Name = nameof(Create))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Create(CreateUserLabResultRequest creationRequest)
    {
        try
        {   
            var userLabResult = new UserLabResult()
            {
                CurrentStep = creationRequest.CurrentStep,
                IsFinished = creationRequest.IsFinished,
                UserId = creationRequest.UserId,
                LabWorkId = creationRequest.LabWorkId
            };
            await userLabResultsService.CreateAsync(userLabResult);
            return StatusCode(201);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Updates an existing user lab result.
    /// </summary>
    /// <param name="updateRequest">The request object containing updated details of the user lab result.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    /// <response code="200">Indicates that the user lab result was successfully updated.</response>
    /// <response code="500">Indicates that an internal server error occurred.</response>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("update", Name = nameof(Update))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(UpdateUserLabResultRequest updateRequest)
    {
        try
        {
            var userLabResult = new UserLabResult()
            {
                CurrentStep = updateRequest.CurrentStep,
                IsFinished = updateRequest.IsFinished,
                UserId = updateRequest.UserId,
                LabWorkId = updateRequest.LabWorkId,
                Id = updateRequest.Id
            };
            await userLabResultsService.UpdateAsync(userLabResult);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves a user lab result by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the user lab result.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the user lab result.</returns>
    /// <response code="200">Indicates that the user lab result was successfully retrieved.</response>
    /// <response code="500">Indicates that an internal server error occurred.</response>
    [HttpGet("get/{id}", Name = nameof(Get))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(UserLabResult), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<UserLabResult>> Get([FromRoute]string id)
    {
        try
        {
            return await userLabResultsService.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all user lab results for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the list of user lab results.</returns>
    /// <response code="200">Indicates that the user lab results were successfully retrieved.</response>
    /// <response code="500">Indicates that an internal server error occurred.</response>
    [HttpGet("get-user's/{userId}", Name = nameof(GetUserResults))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(List<UserLabResult>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<UserLabResult>>> GetUserResults([FromRoute]string userId)
    {
        try
        {
            return await userLabResultsService.GetUserResultsAsync(userId);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all user lab results.
    /// </summary>
    /// <returns>A list of all user lab results.</returns>
    /// <response code="200">Indicates that the user lab results were successfully retrieved.</response>
    [HttpGet("get", Name = nameof(GetAll))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(List<UserLabResult>), 200)]
    public async Task<List<UserLabResult>> GetAll()
    {
        return await userLabResultsService.GetAllAsync();
    }
    
    /// <summary>
    /// Deletes a user lab result by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the user lab result to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    /// <response code="200">Indicates that the user lab result was successfully deleted.</response>
    /// <response code="500">Indicates that an internal server error occurred.</response>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("delete/{id}", Name = nameof(Delete))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete([FromRoute]string id)
    {
        try
        {
            await userLabResultsService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}