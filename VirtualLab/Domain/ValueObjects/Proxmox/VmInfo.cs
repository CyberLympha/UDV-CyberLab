using VirtualLab.Domain.Interfaces.Proxmox;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class VmInfo : IHasNets
{
    public int ProxmoxVmId { get; set; } 
    public string Node { get; set; }
    public NetCollection Nets { get; set; }
}