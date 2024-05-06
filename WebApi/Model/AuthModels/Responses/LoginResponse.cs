using Microsoft.Build.Framework;

namespace WebApi.Model.AuthModels.Responses;

public class LoginResponse
{
    [Required] public User User { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
}