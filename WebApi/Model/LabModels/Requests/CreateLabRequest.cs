using Microsoft.Build.Framework;

namespace WebApi.Model.LabModels.Requests;

public class CreateLabRequest
{
    [Required] public string Id { get; set; } = null!;
}