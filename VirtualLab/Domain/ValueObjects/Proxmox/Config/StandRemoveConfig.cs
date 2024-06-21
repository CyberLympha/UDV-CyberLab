using System.Collections.Immutable;

namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;

public class StandRemoveConfig
{
    public List<VmInfo> VmsInfos { get; } = new();

    public ImmutableList<VmInfo> Vms => VmsInfos.ToImmutableList();
}