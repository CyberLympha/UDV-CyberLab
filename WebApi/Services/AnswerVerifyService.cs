using System.Text.Json;
using WebApi.Model.QuestionModels;

namespace WebApi.Services;

public class AnswerVerifyService
{
    public async Task<bool> IsCorrect(Question question, string answer)
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
                throw new Exception();
        }
    }
}