namespace VirtualLab.Domain.Value_Objects.Proxmox;

public record LaunchVm
{
    public string Node { get; set; }
    public int Qemu { get; set; }
}