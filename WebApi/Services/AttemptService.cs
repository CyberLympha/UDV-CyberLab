using WebApi.Helpers;
using WebApi.Model.AttemptModels;
using WebApi.Model.Repositories;

namespace WebApi.Services;

public class AttemptService
{
    private readonly AnswerVerifyService _answerVerifyService;
    private readonly IRepository<Attempt> _attemptRepository;
    private readonly TestsService _testsService;
    private readonly QuestionsService _questionsService;
    private readonly IdValidationHelper _idValidationHelper;

    public AttemptService(
        TestsService testsService,
        AnswerVerifyService answerVerifyService,
        QuestionsService questionsService,
        IRepository<Attempt> attemptRepository,
        IdValidationHelper idValidationHelper)
    {
        _testsService = testsService;
        _answerVerifyService = answerVerifyService;
        _questionsService = questionsService;
        _attemptRepository = attemptRepository;
        _idValidationHelper = idValidationHelper;
    }

    public async Task<ApiOperationResult<string>> Start(NewAttemptRequest newAttemptRequest)
    {
        var examineeIdValidationResult =
            _idValidationHelper.EnsureValidId(newAttemptRequest.ExamineeId, "Неверный формат ExamineeId");
        if (!examineeIdValidationResult.IsSuccess)
            return examineeIdValidationResult.Error;

        var testIdValidationResult =
            _idValidationHelper.EnsureValidId(newAttemptRequest.TestId, "Неверный формат TestId");
        if (!testIdValidationResult.IsSuccess)
            return testIdValidationResult.Error;

        var attempt = new Attempt
        {
            ExamineeId = newAttemptRequest.ExamineeId,
            TestId = newAttemptRequest.TestId,
            Status = AttemptStatus.InProcess,
            StartTime = DateTime.Now,
            Results = new Dictionary<string, string>()
        };
        return (await _attemptRepository.Create(attempt).ConfigureAwait(false)).Id;
    }

    public async Task<ApiOperationResult> GiveOrChangeTheAnswer(GiveOrChangeTheAnswerAction request)
    {
        var attemptIdValidationResult =
            _idValidationHelper.EnsureValidId(request.AttemptId, "Неверный формат AttemptId");
        if (!attemptIdValidationResult.IsSuccess)
            return attemptIdValidationResult.Error;

        var questionIdValidationResult =
            _idValidationHelper.EnsureValidId(request.QuestionId, "Неверный формат QuestionId");
        if (!questionIdValidationResult.IsSuccess)
            return questionIdValidationResult.Error;

        var attempt = await _attemptRepository.ReadById(request.AttemptId).ConfigureAwait(false);
        if (attempt is null)
            return Error.NotFound("Attempt с таким id не существует");

        var ensureAllowedResult = EnsureAllowed(attempt);
        if (!ensureAllowedResult.IsSuccess)
            return ensureAllowedResult.Error;

        if (attempt.Results.ContainsKey(request.QuestionId))
            attempt.Results[request.QuestionId] = request.Answer;
        else
            attempt.Results.Add(request.QuestionId, request.Answer);

        await _attemptRepository.Update(attempt).ConfigureAwait(false);
        return ApiOperationResult.Success();
    }

    public async Task<ApiOperationResult> End(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        var attempt = await _attemptRepository.ReadById(id).ConfigureAwait(false);
        if (attempt is null)
            return Error.NotFound("Attempt с таким id не существует");

        attempt.Status = AttemptStatus.Ended;
        attempt.EndTime = DateTime.Now;
        await _attemptRepository.Update(attempt).ConfigureAwait(false);
        return ApiOperationResult.Success();
    }

    public async Task<ApiOperationResult<Attempt>> Get(string attemptId)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(attemptId);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return await _attemptRepository.ReadById(attemptId).ConfigureAwait(false);
    }

    public async Task<ApiOperationResult<List<Attempt>>> BatchGet(IEnumerable<string> ids)
    {
        var questions = new List<Attempt>();
        foreach (var id in ids)
        {
            var idValidationResult = _idValidationHelper.EnsureValidId(id);
            if (!idValidationResult.IsSuccess)
                continue;

            var question = await _attemptRepository.ReadById(id).ConfigureAwait(false);
            if (question is not null)
                questions.Add(question);
        }

        return questions;
    }

    public async Task<ApiOperationResult<AttemptResult>> GetResult(string attemptId)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(attemptId);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        var attempt = await _attemptRepository.ReadById(attemptId).ConfigureAwait(false);
        if (attempt is null)
            return Error.NotFound("Attempt с таким id не существует");

        var test = await _testsService.GetById(attempt.TestId).ConfigureAwait(false);
        if (!test.IsSuccess || test.Result is null)
            return Error.NotFound("Test с таким id не существует");

        var result = new AttemptResult
        {
            Results = new Dictionary<string, string>()
        };
        var correctAnswers = 0;
        foreach (var questionId in test.Result.Questions)
        {
            var question = await _questionsService.GetById(questionId).ConfigureAwait(false);
            if (!question.IsSuccess || question.Result is null || !attempt.Results.TryGetValue(questionId, out var answer))
                continue;
            
            var isCorrect = await _answerVerifyService.IsCorrect(question.Result, answer).ConfigureAwait(false);
            if (!isCorrect.IsSuccess)
                return isCorrect.Error;

            result.Results.Add(questionId, $"isCorrect: {isCorrect}");
            if (isCorrect.Result)
                correctAnswers++;
        }

        result.TotalScore = $"Result: {correctAnswers}/{test.Result.Questions.Count}";
        return result;
    }

    public async Task<ApiOperationResult<List<Attempt>>> GetByExamineId(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return (await _attemptRepository.ReadByRule(a => a.ExamineeId == id)).ToList();
    }
    
    public async Task<ApiOperationResult<List<Attempt>>> GetByTestId(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return (await _attemptRepository.ReadByRule(a => a.TestId == id)).ToList();
    }

    private static ApiOperationResult EnsureAllowed(Attempt attempt)
    {
        return attempt.Status == AttemptStatus.Ended
            ? Error.BadRequest("Attempt is ended")
            : ApiOperationResult.Success();
    }
}