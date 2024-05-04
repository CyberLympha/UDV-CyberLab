namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class VirtualMachineInfo
{
    private bool IpEmpty => string.IsNullOrEmpty(Ip);
    
    public bool HasIp => WithVmbr0 && !IpEmpty;
    public int ProxmoxVmId { get; set; }
    public string? Ip { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public bool WithVmbr0 { get; set; }

}