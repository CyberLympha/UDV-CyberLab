using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

public record UpdateInterfaceForVm
{
   public string Node { get; set; }
   public int Qemu { get; set; }
   public NetCollection Nets { get; set; }
}