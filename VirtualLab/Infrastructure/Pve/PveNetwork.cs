using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.ProxmoxStructure;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;
using VirtualLab.Infrastructure.Options;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Infrastructure.Pve;

public class PveNetwork : IProxmoxNetwork // кажется в итоге это будет два отдельных класса)
{
    private readonly PveClient _client;
    private readonly ILog _log;
    private readonly ProxmoxAuthData _proxmoxData;
    private static ApiNetworkErrors ErrorWhen => ApiResultError.WithProxmox.ApiNetworkErrors;

    public PveNetwork(PveClient client, ILog log, ProxmoxAuthData proxmoxData)
    {
        _client = client;
        _log = log;
        _proxmoxData = proxmoxData;
    }

    public async Task<Result<NetCollection>> GetAllNetworksBridgeByVm(int vmId, string node)
    {
        var response = await _client.Nodes[node].Qemu[vmId].Config.VmConfig();

        var listNet = response.Response.data;

        var result = new NetCollection();

        //todo:  здесь ужасная реализация
        foreach (var dnets in listNet)
        {
            if (dnets is not KeyValuePair<string, object> { Key: var key, Value: string netData } ||
                !key.StartsWith("net")) continue;
            
            var data = netData.Split(",");
            var type = data[0].Split("=")[0];
            var iFace = data[1].Split('=')[1];
            result.Add(new NetSettings
            {
                Bridge = iFace,
                Model = type
            });
            
            

            /* убрать если верхняя часть будет работать как нужно
            var net = dnets is KeyValuePair<string, object> ? (KeyValuePair<string, object>)dnets : default;
            if (!net.Key.StartsWith("net")) continue;
            var netSettings = net.Value as string;
            */

            // data ~= virtio=vaef,bridge=vmbr4.
        }

        return result;
    }

    public async Task<Result> Create(string node, Net net)
    {
        var result = await _client.Nodes[node].Network
            .CreateNetwork(net.Bridge, net.Type, autostart: true);

        return result.Match(
            Result.Ok,
            errors => ErrorWhen.Create(errors, node, net.Bridge)
        );
    }


    public async Task<Result> Remove(string node, Net net)
    {
        var result = await _client.Nodes[node].Network[net.Bridge].DeleteNetwork();

        return result.Match(
            Result.Ok,
            errors => ErrorWhen.NetworkDelete(errors, node)
        );
    }

    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.Match(
            Result.Ok,
            errors => ErrorWhen.Apply(errors, node)
        );
    }

    public async Task<Result> UpdateDeviceInterface(string node, int qemu, NetCollection nets)
    {
        var netN = new Dictionary<int, string>(nets.Value);
        var result = await _client.Nodes[node].Qemu[qemu].Config.UpdateVmAsync(netN: netN);

        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.ChangeInterfaceFailure(reasonPhrases, node, qemu)
        );
    }
}