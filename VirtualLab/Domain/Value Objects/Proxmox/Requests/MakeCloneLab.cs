namespace VirtualLab.Domain.Value_Objects.Proxmox.Requests;


// todo: потенциаольно это можно обернуть методами, чтоб было все более интуитивно.
public record CloneVmConfig // такое себе название
{
    public Template Template { get; init; }
    public NetCollection Nets { get; init; }
    public int NewId { get; init; }
}