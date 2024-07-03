namespace WebApi.Model.AttemptModels;

public class AttemptResult
{
    public string TotalScore { get; set; }
    public Dictionary<string, string> Results { get; set; }
}