namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class VirtualMachineInfo
{
    public string? Ip { get; set; }
    public int ProxmoxVmId { get; set; }
    public string Node { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}