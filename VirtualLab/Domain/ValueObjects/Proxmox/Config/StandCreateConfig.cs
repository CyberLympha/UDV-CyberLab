using System.Collections.Immutable;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;

// здесь хранится конфиг именно для одной node. потенциальной лабе может быть задействована не одна node
public record StandCreateConfig // он record здесь мало смысла.
{
    private List<CloneVmConfig> _cloneVmConfig = []; //todo: ваще не нравится это название

    public void Add(CloneVmConfig cloneVmConfig)
    {
        _cloneVmConfig.Add(cloneVmConfig);
    }

    public IEnumerable<Net> GetAllNets()
    {
        return CloneVmConfig.SelectMany(config => config.TemplateData.Nets);
    }
    
    // представим, что у нас точно все вм на одном сервере (node)
    public string Node => _cloneVmConfig[0].TemplateData.Node;
    
    public IEnumerable<CloneVmConfig> CloneVmConfig => _cloneVmConfig.Select(x => x); 
}