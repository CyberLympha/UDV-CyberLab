using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.QuestionModels;
using WebApi.Model.TestModels;
using WebApi.Model.TestModels.Requests;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    private readonly TestsService _testsService;
    private readonly QuestionsService _questionsService;

    public TestsController(TestsService testsService, QuestionsService questionsService)
    {
        _testsService = testsService;
        _questionsService = questionsService;
    }
    
    // public TestsController(TestsService testsService)
    // {
    //     _testsService = testsService;
    // }

    [HttpGet]
    // [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<List<Test>>> Get()
    { 
        return await _testsService.Get;
    }
    
    [HttpGet("{id}")]
    // [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<Test>> GetById(string id)
    {
        return await _testsService.GetById(id);
    }
    
    [HttpPost]
    // [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Post(CreateTestRequest request)
    {
        var test = new Test()
        {
            Description = request.Description,
            Name = request.Name,
            Questions = new List<string>()
        };
        
        foreach (var question in request.Questions)
        {
            var newQuestion = new Question()
            {
                Description = question.Description,
                Text = question.Text,
                QuestionData = JsonSerializer.Serialize(question.QuestionData),
                QuestionType = question.QuestionType
            };
            var questionId = await _questionsService.Create(newQuestion);
            test.Questions.Add(questionId);
        }
    
        var testId = await _testsService.Create(test);
        return Ok(testId);
    }
}