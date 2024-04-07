using MongoDB.Driver;
using WebApi.Model.AttemptModels;
using WebApi.Model.Exceptions;

namespace WebApi.Services;

public class AttemptService
{
    private readonly AnswerVerifyService _answerVerifyService;
    private readonly IMongoCollection<Attempt> _attemptCollection;
    private readonly TestsService _testsService;
    private readonly QuestionsService _questionsService;

    public AttemptService(IMongoCollection<Attempt> attemptCollection, TestsService testsService, AnswerVerifyService answerVerifyService, QuestionsService questionsService)
    {
        _attemptCollection = attemptCollection;
        _testsService = testsService;
        _answerVerifyService = answerVerifyService;
        _questionsService = questionsService;
    }

    public async Task<string> Start(NewAttemptRequest newAttemptRequest)
    {
        var attempt = new Attempt()
        {
            ExamineeId = newAttemptRequest.ExamineeId,
            TestId = newAttemptRequest.TestId,
            Status = AttemptStatus.InProcess,
            StartTime = DateTime.Now,
            Results = new Dictionary<string, string>()
        };
        await _attemptCollection.InsertOneAsync(attempt);
        return attempt.Id;
    }

    public async Task GiveTheAnswer(GiveOrChangeTheAnswerAction request)
    {
        var attempt = (await _attemptCollection.FindAsync(x => x.Id == request.AttemptId)).First();
        EnsureAllowed(attempt);
        attempt.Results.Add(request.QuestionId, request.Answer);
        await _attemptCollection.FindOneAndReplaceAsync(x => x.Id == request.AttemptId, attempt);
    }
    
    public async Task ChangeTheAnswer(GiveOrChangeTheAnswerAction request)
    {
        var attempt = (await _attemptCollection.FindAsync(x => x.Id == request.AttemptId)).First();
        EnsureAllowed(attempt);
        attempt.Results[request.QuestionId] = request.Answer;
        await _attemptCollection.FindOneAndReplaceAsync(x => x.Id == request.AttemptId, attempt);
    }

    public async Task End(string id)
    {
        var attempt = (await _attemptCollection.FindAsync(x => x.Id == id)).First();
        attempt.Status = AttemptStatus.Ended;
        attempt.EndTime = DateTime.Now;
        await _attemptCollection.FindOneAndReplaceAsync(x => x.Id == id, attempt);
    }

    public async Task<Attempt> Get(string attemptId)
    {
        return (await _attemptCollection.FindAsync(x => x.Id == attemptId)).First();
    }
    
    public async Task<AttemptResult> GetResult(string attemptId)
    {
        var attempt = (await _attemptCollection.FindAsync(x => x.Id == attemptId)).First();
        var test = (await _testsService.GetById(attempt.TestId));
        var result = new AttemptResult()
        {
            Results = new Dictionary<string, string>()
        };
        var correctAnswers = 0;
        foreach (var questionId in test.Questions)
        {
            var question = await _questionsService.GetById(questionId);
            var isCorrect = await _answerVerifyService.IsCorrect(question, attempt.Results[questionId]);
            result.Results.Add(questionId, $"isCorrect: {isCorrect}");
            if (isCorrect)
                correctAnswers++;
        }

        result.TotalScore = $"Result: {correctAnswers}/{test.Questions.Count}";

        return result;
    }

    private void EnsureAllowed(Attempt attempt)
    {
        if (attempt.Status == AttemptStatus.Ended)
            throw new AttemptException("Attempt is ended");
    }
}