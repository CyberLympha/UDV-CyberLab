namespace WebApi.Model.AttemptModels;

public class GiveOrChangeTheAnswerAction
{
    public string AttemptId { get; set; }
    public string QuestionId { get; set; }
    public string Answer { get; set; }
}