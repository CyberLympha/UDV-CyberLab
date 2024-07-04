using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.ProxmoxStructure;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IHaveNets // он явно должен находится не в этой директрий ))
{
    public NetCollection Nets { get; }
}