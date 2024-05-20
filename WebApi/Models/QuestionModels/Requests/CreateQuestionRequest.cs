using Microsoft.Build.Framework;

namespace WebApi.Model.QuestionModels.Requests;

public class CreateQuestionRequest
{
    [Required]
    public string Text { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public QuestionType QuestionType { get; set;}
    [Required]
    public Dictionary<string, object> QuestionData { get; set; }
    [Required]
    public string CorrectAnswer { get; set; }
}