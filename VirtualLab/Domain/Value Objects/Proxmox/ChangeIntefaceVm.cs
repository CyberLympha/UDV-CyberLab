namespace VirtualLab.Domain.Value_Objects.Proxmox;

public record ChangeInterfaceForVm
{
   public string Node { get; set; }
   public int Qemu { get; set; }
   public NetCollection Nets { get; set; }
}