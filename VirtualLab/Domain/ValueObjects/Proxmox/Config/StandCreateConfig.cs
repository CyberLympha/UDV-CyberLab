using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;

namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;


// здесь хранится конфиг именно для одной node. потенциальной лабе может быть задействована не одна node
public record StandCreateConfig // он record здесь мало смысла.
{
    public Guid LabId { get; set; }
    public string Node { get; init; } 
    public List<CloneVmConfig> CloneVmConfig { get;  init; } //todo: ваще не нравится это название
    
    public IEnumerable<Net> GetAllNetsInterfaces()
    {
        var hashset = new HashSet<string>();
        foreach (var vmConfig in CloneVmConfig)
        foreach (var vmConfigNet in vmConfig.Nets)
        {
            if (hashset.Contains(vmConfigNet.Bridge)) continue;

            yield return vmConfigNet;
            hashset.Add(vmConfigNet.Bridge);
        }
    }
}