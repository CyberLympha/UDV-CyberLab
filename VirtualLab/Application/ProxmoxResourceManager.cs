using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Infrastructure.Extensions;

namespace VirtualLab.Application;

public class ProxmoxResourceManager : IProxmoxResourceManager
{
    private const int StartPointToTakeIds = 2000; 
    private readonly IProxmoxNode _proxmoxNode;

    public ProxmoxResourceManager(IProxmoxNode proxmoxNode) //todo: заюзать primary
    {
        _proxmoxNode = proxmoxNode;
    }

    public async Task<QemuCollections> GetFreeQemuIds(string node, long count)
    {
        var qemusUnVailabe = await _proxmoxNode.GetAllQemu(node);
        if (!qemusUnVailabe.TryGetValue(out var qemies))
        {
            throw new NotImplementedException("пропиши в ProxmoxResurceManager ошибку");
        }
        
        var freeQemuIds = GetFirstFreeQemuIds(count, qemies);

        return freeQemuIds;
    }

    private static QemuCollections GetFirstFreeQemuIds(long count, IEnumerable<int> qemies)
    {
        var isFirst = true;
        var prevQemuId = int.MinValue;
        var freeQemuIds = new QemuCollections(); 
        foreach (var qemuId in qemies.SkipWhile(x => x <= StartPointToTakeIds))
        {
            if (isFirst)
            {
                prevQemuId = qemuId;
                isFirst = true;
            }
            
            if (qemuId - prevQemuId >= count)
            {
                freeQemuIds.AddRange(prevQemuId, qemuId);
            }

            if (qemuId - prevQemuId < count)
            {
                freeQemuIds.AddRange(prevQemuId, qemuId);
                count -= qemuId - prevQemuId;
            }
        }

        return freeQemuIds;
    }

    public Task<List<long>> GetFreeVmbrs(string node, int count)
    {
        throw new NotImplementedException();
    }
}