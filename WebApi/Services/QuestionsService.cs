using System.Text.Json;
using WebApi.Helpers;
using WebApi.Model.QuestionModels;
using WebApi.Model.QuestionModels.Requests;
using WebApi.Model.Repositories;

namespace WebApi.Services;

public class QuestionsService
{
    private readonly IRepository<Question> _repository;
    private readonly QuestionValidationService _questionValidationService;
    private readonly IdValidationHelper _idValidationHelper;

    public QuestionsService(
        QuestionValidationService questionValidationService,
        IRepository<Question> repository,
        IdValidationHelper idValidationHelper
    )
    {
        _questionValidationService = questionValidationService;
        _repository = repository;
        _idValidationHelper = idValidationHelper;
    }

    public Task<IEnumerable<Question>> Get => _repository.ReadAll();

    public async Task<ApiOperationResult<List<Question>>> BatchGet(IEnumerable<string> ids)
    {
        var questions = new List<Question>();
        foreach (var id in ids)
        {
            var idValidationResult = _idValidationHelper.EnsureValidId(id);
            if (!idValidationResult.IsSuccess)
                continue;

            var question = await GetById(id).ConfigureAwait(false);
            if (question is { IsSuccess: true, Result: { } })
                questions.Add(question.Result);
        }

        return questions;
    }

    public async Task<ApiOperationResult<Question>> GetById(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return await _repository.ReadById(id).ConfigureAwait(false);
    }

    public async Task<ApiOperationResult<string>> Create(Question question)
    {
        var validationResult = _questionValidationService.EnsureValid(question);
        if (!validationResult.IsSuccess)
            return validationResult.Error;

        var newQuestion = await _repository.Create(question).ConfigureAwait(false);
        return newQuestion.Id;
    }

    public async Task<ApiOperationResult> Update(CreateQuestionRequest question, string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        var newQuestion = new Question
        {
            Description = question.Description,
            QuestionData = JsonSerializer.Serialize(question.QuestionData),
            Id = id,
            QuestionType = question.QuestionType,
            Text = question.Text
        };
        var result = await _repository.Update(newQuestion).ConfigureAwait(false);
        if (result is null)
            return Error.NotFound("Question с таким id не существует");
        return ApiOperationResult.Success();
    }

    public async Task<ApiOperationResult> Delete(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return await _repository.Delete(id).ConfigureAwait(false);
    }
}