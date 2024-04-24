using VirtualLab.Domain.Value_Objects.Proxmox.Requests;

namespace VirtualLab.Domain.Value_Objects.Proxmox;


// здесь хранится конфиг именно для одной node. потенциальной лабе может быть задействована не одна node
public record LabCreateRequest // он record здесь мало смысла.
{
    public string Node { get; init; } 
    public List<CloneRequest> ClonesRequest { get;  init; }
    public NetCollection Nets { get; init; }
    
}