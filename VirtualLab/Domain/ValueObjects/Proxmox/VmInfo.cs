namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class VmInfo
{
    public int ProxmoxId { get; set; } 
    public string Node { get; set; }
    public NetCollection Nets { get; set; }
    
}