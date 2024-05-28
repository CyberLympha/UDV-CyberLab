using System.Text.Json;
using WebApi.Model.QuestionModels;
using WebApi.Model.QuestionModels.Requests;
using WebApi.Model.Repositories;

namespace WebApi.Services;

public class QuestionsService
{
    private readonly IRepository<Question> _repository;
    private readonly QuestionValidationService _questionValidationService;

    public QuestionsService(QuestionValidationService questionValidationService, IRepository<Question> repository)
    {
        Console.WriteLine("15");
        
        _questionValidationService = questionValidationService;
        _repository = repository;
    }

    public Task<IEnumerable<Question>> Get => _repository.ReadAll();

    public Task<Question> GetById(string id) => _repository.ReadById(id);

    public async Task<string> Create(Question question)
    {
        Console.WriteLine("27");
        
        var newQuestion = await _repository.Create(question);
        // _questionValidationService.EnsureValid(question);
        return newQuestion.Id;
    }

    public async Task Update(CreateQuestionRequest question, string id)
    {
        Console.WriteLine("36");
        
        var newQuestion = new Question()
        {
            Description = question.Description,
            QuestionData = JsonSerializer.Serialize(question.QuestionData),
            Id = id,
            QuestionType = question.QuestionType,
            Text = question.Text
        };
        await _repository.Update(newQuestion);
    }

    public async Task Delete(string id)
    {
        Console.WriteLine("51");
        
        _repository.Delete(id);
    }
}