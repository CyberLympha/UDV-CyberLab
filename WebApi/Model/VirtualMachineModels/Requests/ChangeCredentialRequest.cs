using Microsoft.Build.Framework;

namespace WebApi.Model.VirtualMachineModels.Requests;

public class ChangeCredentialsRequest
{
    [Required] public string Vmid { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Username { get; set; } = null!;
    [Required] public string SshKey { get; set; } = null!;
}