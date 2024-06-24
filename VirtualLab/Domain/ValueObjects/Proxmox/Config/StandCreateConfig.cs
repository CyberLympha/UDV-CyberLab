using System.Collections.Immutable;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;

namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;

// здесь хранится конфиг именно для одной node. потенциальной лабе может быть задействована не одна node
public record StandCreateConfig // он record здесь мало смысла.
{
    public string Node { get; set; }
    private List<CloneVmConfig> _cloneVmConfig = []; //todo: ваще не нравится это название

    public void Add(CloneVmConfig cloneVmConfig)
    {
        _cloneVmConfig.Add(cloneVmConfig);
    }
    public ImmutableList<CloneVmConfig> CloneVmConfig => _cloneVmConfig.ToImmutableList(); 
}