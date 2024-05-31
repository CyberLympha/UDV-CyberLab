using System.Net;
using System.Text.Json;
using WebApi.Helpers;
using WebApi.Model.QuestionModels;

namespace WebApi.Services;

public class AnswerVerifyService
{
    public async Task<ApiOperationResult<bool>> IsCorrect(Question question, string answer)
    {
        switch (question.QuestionType)
        {
            case QuestionType.Radio:
                return answer == question.CorrectAnswer;
            case QuestionType.CheckBox:
            {
                var correctAnswer =
                    JsonSerializer.Deserialize<List<string>>(question.CorrectAnswer)!.ToHashSet();
                var currentAnswer =
                    JsonSerializer.Deserialize<List<string>>(answer)!.ToHashSet();
                return currentAnswer.Equals(correctAnswer);
            }
            case QuestionType.Text:
                return answer == question.CorrectAnswer;
            default:
                return ApiOperationResult<bool>.Failure(HttpStatusCode.InternalServerError, "Неизвестный QuestionType");
        }
    }
}