using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Infrastructure.ApiResult;
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

    public async Task<Result<QemuCollections>> GetFreeQemuIds(string node, long count)
    {
        var qemusUnVailabe = await _proxmoxNode.GetAllQemu(node);
        if (!qemusUnVailabe.TryGetValue(out var qemies, out var errors))
            return Result.Fail(errors);

        var freeQemuIds = GetFirstFreeQemuIds(count, qemies);

        return freeQemuIds;
    }

    public Task<Result<List<long>>> GetFreeVmbrs(string node, int count)
    {
        throw new NotImplementedException();
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

            if (qemuId - prevQemuId >= count) freeQemuIds.AddRange(prevQemuId, qemuId);

            if (qemuId - prevQemuId < count)
            {
                freeQemuIds.AddRange(prevQemuId + 1, qemuId - 1);
                count -= qemuId - prevQemuId;
            }
        }

        var last = qemies.Last(); //todo: O(N) БРООООО
        last = last > 2000 ? last : 2000;
        while (freeQemuIds.Count != count)
        {
            if (freeQemuIds.Count != count)
            {
                freeQemuIds.Add(++last);
            }
        }

        return freeQemuIds;
    }
}