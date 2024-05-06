using MongoDB.Driver;
using WebApi.Model.AttemptModels;
using WebApi.Model.Exceptions;
using WebApi.Model.Repositories;

namespace WebApi.Services;

public class AttemptService
{
    private readonly AnswerVerifyService _answerVerifyService;
    private readonly IRepository<Attempt> _attemptRepository;
    private readonly TestsService _testsService;
    private readonly QuestionsService _questionsService;

    public AttemptService(
        TestsService testsService, 
        AnswerVerifyService answerVerifyService, 
        QuestionsService questionsService, 
        IRepository<Attempt> attemptRepository
        )
    {
        _testsService = testsService;
        _answerVerifyService = answerVerifyService;
        _questionsService = questionsService;
        _attemptRepository = attemptRepository;
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
        return (await _attemptRepository.Create(attempt)).Id;
    }

    public async Task GiveTheAnswer(GiveOrChangeTheAnswerAction request)
    {
        var attempt = await _attemptRepository.ReadById(request.AttemptId);
        EnsureAllowed(attempt);
        attempt.Results.Add(request.QuestionId, request.Answer);
        await _attemptRepository.Update(attempt);
    }
    
    public async Task ChangeTheAnswer(GiveOrChangeTheAnswerAction request)
    {
        var attempt = await _attemptRepository.ReadById(request.AttemptId);
        EnsureAllowed(attempt);
        attempt.Results[request.QuestionId] = request.Answer;
        await _attemptRepository.Update(attempt);
    }

    public async Task End(string id)
    {
        var attempt = await _attemptRepository.ReadById(id);
        attempt.Status = AttemptStatus.Ended;
        attempt.EndTime = DateTime.Now;
        await _attemptRepository.Update(attempt);
    }

    public async Task<Attempt> Get(string attemptId)
    {
        return await _attemptRepository.ReadById(attemptId);
    }
    
    public async Task<AttemptResult> GetResult(string attemptId)
    {
        var attempt = await _attemptRepository.ReadById(attemptId);;
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