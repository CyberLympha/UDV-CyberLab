using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
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


    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<List<Test>>> Get()
    {
        return (await _testsService.Get.ConfigureAwait(false)).ToList();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<Test>> GetById(string id)
    {
        var result = await _testsService.GetById(id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<string>> Post(CreateTestRequest request)
    {
        var test = new Test
        {
            Description = request.Description,
            Name = request.Name,
            Questions = new List<string>()
        };

        foreach (var question in request.Questions)
        {
            var newQuestion = new Question
            {
                Description = question.Description,
                Text = question.Text,
                QuestionData = JsonSerializer.Serialize(question.QuestionData),
                CorrectAnswer = question.CorrectAnswer,
                QuestionType = question.QuestionType
            };
            var questionIdResult = await _questionsService.Create(newQuestion).ConfigureAwait(false);
            if (!questionIdResult.IsSuccess)
                return questionIdResult.ToActionResult();
            test.Questions.Add(questionIdResult.Result);
        }

        var testId = await _testsService.Create(test).ConfigureAwait(false);
        return testId.ToActionResult();
    }
}