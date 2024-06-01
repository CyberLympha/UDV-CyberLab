using Microsoft.Build.Framework;

namespace WebApi.Model.AuthModels.Requests;

public class RegistrationRequest
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string SecondName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}