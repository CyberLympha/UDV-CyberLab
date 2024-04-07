using Microsoft.AspNetCore.Mvc;
using WebApi.Model.AttemptModels;
using WebApi.Model.Exceptions;
using WebApi.Services;

namespace WebApi.Controllers;

public class AttemptController : ControllerBase
{
    private readonly AttemptService _attemptService;

    public AttemptController(AttemptService attemptService)
    {
        _attemptService = attemptService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start(NewAttemptRequest request)
    {
        var attemptId = await _attemptService.Start(request);
        return Ok(attemptId);
    }
    
    [HttpPost("{id}/give_the_answer")]
    public async Task<IActionResult> GiveTheAnswer(GiveOrChangeTheAnswerRequest request, string id)
    {
        try
        {
            var giveTheAnswerAction = new GiveOrChangeTheAnswerAction()
            {
                AttemptId = id,
                QuestionId = request.QuestionId,
                Answer = request.Answer
            };
            await _attemptService.GiveTheAnswer(giveTheAnswerAction);
            return Ok();
        }
        catch (AttemptException e)
        {
            return BadRequest(e);
        }
    }
    [HttpPost("{id}/change_the_answer")]
    public async Task<IActionResult> ChangeTheAnswer(GiveOrChangeTheAnswerRequest request, string id)
    {
        try
        {
            var changeTheAnswerAction = new GiveOrChangeTheAnswerAction()
            {
                AttemptId = id,
                QuestionId = request.QuestionId,
                Answer = request.Answer
            };
            await _attemptService.ChangeTheAnswer(changeTheAnswerAction);
            return Ok();
        }
        catch (AttemptException e)
        {
            return BadRequest(e);
        }
    }
    [HttpPost("end")]
    public async Task<IActionResult> End(string id)
    {
        await _attemptService.End(id);
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _attemptService.Get(id));
    }

    [HttpGet("{id}/result")]
    public async Task<IActionResult> GetResult(string id)
    {
        return Ok(await _attemptService.GetResult(id));
    }
}