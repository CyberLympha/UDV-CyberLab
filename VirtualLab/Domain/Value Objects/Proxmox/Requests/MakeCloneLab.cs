namespace VirtualLab.Domain.Value_Objects.Proxmox.Requests;

public record CloneRequest // такое себе название
{
    public Template Template { get; init; }
    public int NewId { get; init; }
}