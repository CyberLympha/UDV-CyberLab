using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class LoginRequest
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

public class LoginResponse
{
    [Required] public User User { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
}

public class RegistrationRequest
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string SecondName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

public class ChangeCredentialsRequest
{
    [Required] public string Vmid { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Username { get; set; } = null!;
    [Required] public string SshKey { get; set; } = null!;
}

public enum VmType
{
    Kali,
    Windows,
    Ubuntu
    
}

public class CreateLabRequest
{
    [Required] public string Id { get; set; } = null!;
}

public class CreateNewItem
{
    [Required] public string title { get; set; } = null!;
    [Required] public string text { get; set; } = null!;
    [Required] public string createdAt { get; set; } = null!;
}