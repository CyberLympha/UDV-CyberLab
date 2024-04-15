using System.Text.Json;
using MongoDB.Driver;
using WebApi.Model.QuestionModels;
using WebApi.Model.QuestionModels.Requests;

namespace WebApi.Services;

public class QuestionsService
{
    private readonly IMongoCollection<Question> _questionsCollection;
    private readonly QuestionValidationService _questionValidationService;

    public QuestionsService(IMongoCollection<Question> questionsCollection,
        QuestionValidationService questionValidationService)
    {
        _questionsCollection = questionsCollection;
        _questionValidationService = questionValidationService;
    }
    //
    // public QuestionsService(IMongoCollection<Question> questionsCollection)
    // {
    //     _questionsCollection = questionsCollection;
    // }

    public Task<List<Question>> Get
    {
        get { return _questionsCollection.Find(_ => true).ToListAsync(); }
    }

    public Task<Question> GetById(string id)
    {
        return _questionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<string> Create(Question question)
    {
        _questionValidationService.EnsureValid(question);
        await _questionsCollection.InsertOneAsync(question);
        return question.Id;
    }

    public async Task Update(CreateQuestionRequest question, string id)
    {
        var n = new Question()
        {
            Description = question.Description,
            QuestionData = JsonSerializer.Serialize(question.QuestionData),
            Id = id,
            QuestionType = question.QuestionType,
            Text = question.Text
        };
        await _questionsCollection.FindOneAndReplaceAsync(x => x.Id == id, n);
    }

    public async Task Delete(string id)
    {
        await _questionsCollection.DeleteOneAsync(x => x.Id == id);
    }
}