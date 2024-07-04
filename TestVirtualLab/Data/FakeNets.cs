using FluentResults;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.ProxmoxStructure;

namespace TestVirtualLab.Data;

public static class FakeNets
{
    public static async Task<Result<NetCollection>> Create()
    {
        return new NetCollection() 
        {
            new NetSettings()
            {
                Bridge = "vmbr0", Model = "virtio"
            },
            new NetSettings()
            {
                Bridge = "vmbr2", Model = "virtio"
            }
        };
    }  
}