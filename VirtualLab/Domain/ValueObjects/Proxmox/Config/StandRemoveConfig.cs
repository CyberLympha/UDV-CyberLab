using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;

public class StandRemoveConfig
{
    public List<VmInfo> VmsData { get; set; } = new();
    
    
    //todo дубликаты в обоих конфигах
    public IEnumerable<Net> GetAllNetsInterfaces()
    {
        var hashset = new HashSet<string>();
        foreach (var vmConfig in VmsData)
        foreach (var vmConfigNet in vmConfig.Nets)
        {
            if (hashset.Contains(vmConfigNet.Bridge)) continue;

            yield return vmConfigNet;
            hashset.Add(vmConfigNet.Bridge);
        }
    }
}