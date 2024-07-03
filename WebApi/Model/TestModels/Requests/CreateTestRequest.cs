using Microsoft.Build.Framework;
using WebApi.Model.QuestionModels.Requests;

namespace WebApi.Model.TestModels.Requests;

public class CreateTestRequest
{
    [Required] 
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; }
    [Required]
    public List<CreateQuestionRequest> Questions { get; set; }
}