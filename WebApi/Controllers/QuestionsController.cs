using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.QuestionModels;
using WebApi.Model.QuestionModels.Requests;
using WebApi.Services;

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
    // [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<List<Question>>> Get()
    { 
        return await _questionsService.Get;
    }
    
    [HttpGet("{id}")]
    // [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<Question>> GetById(string id)
    { 
        return await _questionsService.GetById(id);
    }
    
    [HttpPost]
    // [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Post(CreateQuestionRequest request)
    {
        var question = new Question()
        {
            Description = request.Description,
            Text = request.Text,
            QuestionData = JsonSerializer.Serialize(request.QuestionData),
            QuestionType = request.QuestionType
        };
        await _questionsService.Create(question);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(CreateQuestionRequest request, string id)
    {
        await _questionsService.Update(request, id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        await _questionsService.Delete(id);
        return Ok();
    }
}