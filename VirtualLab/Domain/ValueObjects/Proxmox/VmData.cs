namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class VmData
{
    public int ProxmoxId { get; set; } 
    public NetCollection Nets { get; set; }
}