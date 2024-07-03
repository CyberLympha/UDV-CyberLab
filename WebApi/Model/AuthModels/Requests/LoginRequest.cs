using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.AuthModels.Requests;

public class LoginRequest
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}