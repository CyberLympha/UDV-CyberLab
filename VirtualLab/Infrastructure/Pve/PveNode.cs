using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Infrastructure.ApiResult;
using Result = FluentResults.Result;

namespace VirtualLab.Infrastructure.Pve;

public class PveNode : IProxmoxNode
{
    private readonly PveClient _client;
    private static NodeApiResult ErrorWhen => ApiResultError.WithProxmox.Node;
    public PveNode(PveClient client)
    {
        _client = client;
    }

    public async Task<Result<List<int>>> GetAllQemu(string node)
    {
        var result = await _client.Nodes[node].Qemu.Vmlist(false);

        if (!result.IsSuccessStatusCode)
            return Result.Fail(ErrorWhen.GetQuemies(node, result.ReasonPhrase));
        
        var vmids = new List<int>();
        var vmsData = result.Response.data;
        foreach (var vmData in vmsData)
        foreach (var vmElementData in vmData)
            if (vmElementData is KeyValuePair<string, object> { Key: "vmid", Value: long id })
                vmids.Add((int)id);

        vmids.Sort();
        return vmids;
    }
}