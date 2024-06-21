using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IHaveNets // он явно должен находится не в этой директрий ))
{
    public NetCollection Nets { get; }
}