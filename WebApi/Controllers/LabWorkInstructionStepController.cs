using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.LabWorks;
using WebApi.Services;

namespace WebApi.Controllers;

/// <summary>
///     Controller for handling lab work instruction's steps.
/// </summary>
[Route("/api/lab-work-instruction")]
[ApiController]
public class LabWorkInstructionStepController : ControllerBase
{
    private readonly InstructionStepsService instructionStepsService;
    private readonly LabWorkInstructionService labWorkInstructionService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LabWorkInstructionStepController" /> class.
    /// </summary>
    /// <param name="labWorkInstructionService">The lab work instruction service.</param>
    /// <param name="instructionStepsService">The lab work instruction's steps service.</param>
    public LabWorkInstructionStepController(
        LabWorkInstructionService labWorkInstructionService,
        InstructionStepsService instructionStepsService)
    {
        this.labWorkInstructionService = labWorkInstructionService;
        this.instructionStepsService = instructionStepsService;
    }

    /// <summary>
    ///     Retrieves the instruction for a specific step in a lab work.
    /// </summary>
    /// <param name="instructionId">The identifier of the lab work instruction.</param>
    /// <param name="number">The step number for which the instruction is requested.</param>
    /// <returns>The instruction for the specified step as a string.</returns>
    /// <response code="200">Returns the instruction for the specified step.</response>
    /// <response code="404">If the instruction or step is not found.</response>
    [HttpGet("get/{instructionId}/{number}", Name = nameof(GetStepInstruction))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<string>> GetStepInstruction([FromRoute] string instructionId,
        [FromRoute] string number)
    {
        try
        {
            return await labWorkInstructionService.GetStepInstructionAsync(instructionId, number);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Checks if a specified step is the last step in a lab work instruction.
    /// </summary>
    /// <param name="instructionId">The identifier of the lab work instruction.</param>
    /// <param name="number">The step number to check.</param>
    /// <returns>True if the step is the last step; otherwise, false.</returns>
    /// <response code="200">Returns true if the step is the last step.</response>
    /// <response code="500">If an error occurs during the operation.</response>
    [HttpGet("is-last/{instructionId}/{number}", Name = nameof(IsStepLast))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<bool>> IsStepLast([FromRoute] string instructionId, [FromRoute] string number)
    {
        try
        {
            return await labWorkInstructionService.IsStepLastAsync(instructionId, number);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves the hint for a specific step in a lab work.
    /// </summary>
    /// <param name="instructionId">The identifier of the lab work instruction.</param>
    /// <param name="number">The step number for which the hint is requested.</param>
    /// <returns>The hint for the specified step as a string.</returns>
    /// <response code="200">Returns the hint for the specified step.</response>
    /// <response code="500">If an error occurs during the operation.</response>
    [HttpGet("get-hint/{instructionId}/{number}", Name = nameof(GetStepHint))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<string>> GetStepHint([FromRoute] string instructionId, [FromRoute] string number)
    {
        try
        {
            return await labWorkInstructionService.GetStepHintAsync(instructionId, number);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves the number of steps in a lab work instruction.
    /// </summary>
    /// <param name="instructionId">The ID of the lab work instruction.</param>
    /// <returns>The number of steps in the instruction.</returns>
    /// <response code="200">Returns the number of steps in the instruction.</response>
    /// <response code="500">If an error occurs during the operation.</response>
    [HttpGet("get-amount/{instructionId}", Name = nameof(GetStepsAmount))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<int>> GetStepsAmount([FromRoute] string instructionId)
    {
        try
        {
            return await labWorkInstructionService.GetStepsAmountAsync(instructionId);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Checks the user's answer for a lab work task.
    /// </summary>
    /// <remarks>
    ///     Accepts the user ID, lab work ID, and task number.
    /// </remarks>
    /// <param name="userId">The user ID.</param>
    /// <param name="labId">The lab work ID.</param>
    /// <param name="number">The task number.</param>
    /// <returns>
    ///     The result of the answer check. True if the answer is correct, otherwise False.
    /// </returns>
    /// <response code="200">Successful request. Returns the result of the answer check.</response>
    /// <response code="500">Internal server error. Returns an error message.</response>
    [HttpGet("check-answer/{userId}/{labId}/{number}", Name = nameof(CheckAnswer))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<bool>> CheckAnswer([FromRoute] string userId, [FromRoute] string labId,
        [FromRoute] string number)
    {
        try
        {
            return await labWorkInstructionService.CheckAnswer(userId, labId, number);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Creates a new instruction step.
    /// </summary>
    /// <param name="creationRequest">The request containing instruction step details.</param>
    /// <returns>A status code indicating the result of the operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("step/create", Name = nameof(CreateStep))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> CreateStep(CreateInstructionStepRequest creationRequest)
    {
        try
        {
            var instructionStep = new InstructionStep
            {
                Answers = creationRequest.Answers,
                Hint = creationRequest.Hint,
                Instruction = creationRequest.Instruction
            };
            await instructionStepsService.CreateAsync(instructionStep);
            return StatusCode(201);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Updates an existing instruction step.
    /// </summary>
    /// <param name="updateRequest">The request containing updated instruction step details.</param>
    /// <returns>A status code indicating the result of the operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("step/update", Name = nameof(UpdateStep))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> UpdateStep(UpdateInstructionStepRequest updateRequest)
    {
        try
        {
            var instructionStep = new InstructionStep
            {
                Id = updateRequest.Id,
                Answers = updateRequest.Answers,
                Hint = updateRequest.Hint,
                Instruction = updateRequest.Instruction
            };
            await instructionStepsService.UpdateAsync(instructionStep);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves an instruction step by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the instruction step.</param>
    /// <returns>The requested instruction step.</returns>
    [HttpGet("step/get/{id}", Name = nameof(GetStep))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(InstructionStep), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<InstructionStep>> GetStep([FromRoute] string id)
    {
        try
        {
            return await instructionStepsService.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Retrieves all instruction steps.
    /// </summary>
    /// <returns>A list of all instruction steps.</returns>
    [HttpGet("step/get", Name = nameof(GetAllSteps))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(List<InstructionStep>), 200)]
    public async Task<List<InstructionStep>> GetAllSteps()
    {
        return await instructionStepsService.GetAllAsync();
    }

    /// <summary>
    ///     Deletes an instruction step by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the instruction step.</param>
    /// <returns>A status code indicating the result of the operation.</returns>
    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost("step/delete/{id}", Name = nameof(DeleteStep))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteStep([FromRoute] string id)
    {
        try
        {
            await instructionStepsService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}