namespace WebApi.Model.AttemptModels;

public class GiveOrChangeTheAnswerRequest
{
    public string QuestionId { get; set; }
    public string Answer { get; set; }
}