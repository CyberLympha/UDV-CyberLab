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
                try
                {
                    var correctAnswer =
                        JsonSerializer.Deserialize<List<string>>(question.CorrectAnswer)!.ToHashSet();
                    var currentAnswer =
                        JsonSerializer.Deserialize<List<string>>(answer)!.ToHashSet();
                    return currentAnswer.Equals(correctAnswer);
                }
                catch
                {
                    return Error.InternalError("Ошибка при десериализации ответа");
                }
            }
            case QuestionType.Text:
                return answer == question.CorrectAnswer;
            default:
                return Error.InternalError("Неизвестный QuestionType");
        }
    }
}