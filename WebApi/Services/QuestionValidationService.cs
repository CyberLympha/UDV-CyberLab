using System.Text.Json;
using WebApi.Model.Exceptions;
using WebApi.Model.QuestionModels;

namespace WebApi.Services;

public class QuestionValidationService
{
    public void EnsureValid(Question question)
    {
        if (question.QuestionType == QuestionType.Radio)
            IsRadioQuestionValid(question);
        if (question.QuestionType == QuestionType.CheckBox)
            IsCheckBoxQuestionValid(question);
        if (question.QuestionType == QuestionType.Text)
            return;
    }

    private void IsCheckBoxQuestionValid(Question question)
    {
        Dictionary<string, string>? data;
        try
        {
            data = JsonSerializer.Deserialize<Dictionary<string, string>>(question.QuestionData);
        }
        catch
        {
            throw new IncorrectDataException("Поле QuestionData должно являться Dictionary<string, string>");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(data!["Variants"]);
        }
        catch
        {
            throw new IncorrectDataException("В QuestionData должно должно быть поле Variants");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(question.CorrectAnswer);
        }
        catch
        {
            throw new IncorrectDataException("CorrectAnswer должен иметь вид List<string>");
        }
    }

    private void IsRadioQuestionValid(Question question)
    {
        Dictionary<string, string>? data;
        try
        {
            data = JsonSerializer.Deserialize<Dictionary<string, string>>(question.QuestionData);
        }
        catch
        {
            throw new IncorrectDataException("Поле QuestionData должно являться Dictionary<string, string>");
        }

        try
        {
            JsonSerializer.Deserialize<List<string>>(data!["Variants"]);
        }
        catch
        {
            throw new IncorrectDataException("В QuestionData должно должно быть поле Variants");
        }
    }
}