using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IHasNets
{
    public NetCollection Nets { get;  }
}