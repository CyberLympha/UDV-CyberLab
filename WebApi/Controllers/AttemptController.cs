using Microsoft.AspNetCore.Mvc;
using WebApi.Model.AttemptModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AttemptController : ControllerBase
{
    private readonly AttemptService _attemptService;

    public AttemptController(AttemptService attemptService)
    {
        _attemptService = attemptService;
    }

    [HttpPost("start")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<string>> Start(NewAttemptRequest request)
    {
        var attemptStartResult = await _attemptService.Start(request).ConfigureAwait(false);
        return attemptStartResult.ToActionResult();
    }

    [HttpPost("{id}/give_the_answer")]
    [HttpPost("{id}/change_the_answer")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<IActionResult> GiveOrChangeTheAnswer(GiveOrChangeTheAnswerRequest request, string id)
    {
        var giveTheAnswerAction = new GiveOrChangeTheAnswerAction
        {
            AttemptId = id,
            QuestionId = request.QuestionId,
            Answer = request.Answer
        };
        var result = await _attemptService.GiveOrChangeTheAnswer(giveTheAnswerAction).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost("{id}/end")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<IActionResult> End(string id)
    {
        var result = await _attemptService.End(id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<Attempt>> Get(string id)
    {
        var result = await _attemptService.Get(id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost("batchGet")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<List<Attempt>>> BatchGet([FromBody] string[] ids)
    {
        var result = await _attemptService.BatchGet(ids).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpGet("{id}/result")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<AttemptResult>> GetResult(string id)
    {
        var result = await _attemptService.GetResult(id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpGet("byExamineId/{id}")]
    public async Task<ActionResult<List<Attempt>>> GetAttemptsByExamineId(string id)
    {
        var result = await _attemptService.GetByExamineId(id);
        return result.ToActionResult();
    }

    [HttpGet("byTestId/{id}")]
    public async Task<ActionResult<List<Attempt>>> GetAttemptsByTestId(string id)
    {
        var result = await _attemptService.GetByTestId(id);
        return result.ToActionResult();
    }
}