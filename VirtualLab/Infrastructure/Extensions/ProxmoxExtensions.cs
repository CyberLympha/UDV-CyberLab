using System.Collections.Immutable;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxExtensions 
{
    public static IEnumerable<Net> GetAllNets<T>(this ImmutableList<T> haveNets) where T: IHaveNets
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
}