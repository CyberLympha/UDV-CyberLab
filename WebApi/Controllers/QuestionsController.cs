using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.QuestionModels;
using WebApi.Model.QuestionModels.Requests;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly QuestionsService _questionsService;

    public QuestionsController(QuestionsService questionsService)
    {
        _questionsService = questionsService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<List<Question>>> Get()
    {
        return (await _questionsService.Get.ConfigureAwait(false)).ToList();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<Question>> GetById(string id)
    {
        var result = await _questionsService.GetById(id).ConfigureAwait(false);
        return result.ToActionResult();
    }


    [HttpPost("batchGet")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<List<Question>>> BatchGet(string[] ids)
    {
        var result = await _questionsService.BatchGet(ids).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<string>> Post(CreateQuestionRequest request)
    {
        var question = new Question
        {
            Description = request.Description,
            Text = request.Text,
            QuestionData = JsonSerializer.Serialize(request.QuestionData),
            QuestionType = request.QuestionType
        };
        var result = await _questionsService.Create(question).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Put(CreateQuestionRequest request, string id)
    {
        var result = await _questionsService.Update(request, id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _questionsService.Delete(id).ConfigureAwait(false);
        return result.ToActionResult();
    }
}