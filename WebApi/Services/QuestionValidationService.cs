using System.Text.Json;
using WebApi.Helpers;
using WebApi.Model.QuestionModels;

namespace WebApi.Services;

public class QuestionValidationService
{
    public ApiOperationResult EnsureValid(Question question)
    {
        return question.QuestionType switch
        {
            QuestionType.Radio => IsRadioQuestionValid(question),
            QuestionType.CheckBox => IsCheckBoxQuestionValid(question),
            _ => ApiOperationResult.Success()
        };
    }

    private ApiOperationResult IsCheckBoxQuestionValid(Question question)
    {
        Dictionary<string, string>? data;
        try
        {
            data = JsonSerializer.Deserialize<Dictionary<string, string>>(question.QuestionData);
        }
        catch
        {
            return Error.BadRequest("Поле QuestionData должно являться Dictionary<string, string>");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(data!["Variants"]);
        }
        catch
        {
            return Error.BadRequest("В QuestionData должно должно быть поле Variants");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(question.CorrectAnswer);
        }
        catch
        {
            return Error.BadRequest("CorrectAnswer должен иметь вид List<string>");
        }

        return ApiOperationResult.Success();
    }

    private ApiOperationResult IsRadioQuestionValid(Question question)
    {
        Dictionary<string, string>? data;
        try
        {
            data = JsonSerializer.Deserialize<Dictionary<string, string>>(question.QuestionData);
        }
        catch
        {
            return Error.BadRequest("Поле QuestionData должно являться Dictionary<string, string>");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(data!["Variants"]);
        }
        catch
        {
            return Error.BadRequest("В QuestionData должно должно быть поле Variants");
        }

        return ApiOperationResult.Success();
    }
}