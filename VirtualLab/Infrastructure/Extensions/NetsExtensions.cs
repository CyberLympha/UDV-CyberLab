using System.Collections.Immutable;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure.Extensions;

public static class NetsExtensions 
{
    public static IEnumerable<Net> GetAllNets<T>(this IList<T> haveNets) where T: IHaveNets
    {
        var hashset = new HashSet<string>();
        foreach (var vmConfig in haveNets)
        foreach (var vmConfigNet in vmConfig.Nets)
        {
            if (hashset.Contains(vmConfigNet.Bridge)) continue;

            yield return vmConfigNet;
            hashset.Add(vmConfigNet.Bridge);
        }
    }

    public static bool WithNets<T>(this T config) where T: IHaveNets
    {
        return config.Nets.Count != 0;
    }

    public static IEnumerable<Net> WithoutVmbr0(this IEnumerable<Net> nets)
    {
        return nets.Where(x => x.Bridge != "vmbr0");
    }

    public static bool HaveVmbr0(this IEnumerable<Net> nets) => nets.Any(n => n.Bridge == "vmbr0");
}