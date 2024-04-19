namespace VirtualLab.Domain.Value_Objects;

public record CloneVmTemplate // такое себе название
{
    public int VmIdTemplate { get; set; }
    public int NewId { get; set; }
}