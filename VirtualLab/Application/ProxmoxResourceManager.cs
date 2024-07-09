using FluentResults;
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

    public async Task<Result<List<int>>> GetFreeQemuIds(string node, long count)
    {
        var qemusUnVailabe = await _proxmoxNode.GetAllQemuIds(node);
        if (!qemusUnVailabe.TryGetValue(out var qemies, out var errors))
            return Result.Fail(errors);

        var freeQemuIds = GetFirstFreeIds(count, qemies);

        return freeQemuIds;
    }

    public async Task<Result<List<int>>> GetFreeVmbrs(string node, int count)
    {
        if (!(await _proxmoxNode.GetAllIFaceId(node)).TryGetValue(out var busyIFaceIds, out var errors))
            return Result.Fail(errors);

        var freeIds = GetFirstFreeIds(count, busyIFaceIds);

        return freeIds;
    }

    // todo: переписать самое важное сейчас
    private static List<int> GetFirstFreeIds(long count, IReadOnlyList<int> qemies)
    {
        var isFirst = true;
        var prevId = int.MinValue;
        var freeQemuIds = new List<int>();
        foreach (var curId in qemies.SkipWhile(x => x <= StartPointToTakeIds))
        {
            if (isFirst)
            {
                prevId = curId;
                isFirst = true;
            }

            if (curId - prevId >= count)
            {
                for (var i = prevId; i <= curId; i++) freeQemuIds.Add(i);
            }

            if (curId - prevId < count)
            {
                for (var i = prevId + 1; i <= curId - 1; i++) freeQemuIds.Add(i);
                count -= curId - prevId;
            }
        }

        var last = qemies.Max(); //todo: O(N) БРООООО
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