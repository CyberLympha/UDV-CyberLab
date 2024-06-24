using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Infrastructure.Pve;

public class Proxmox : IProxmoxVm, IProxmoxNetwork, IProxmoxNode // кажется в итоге это будет два отдельных класса)
{
    private readonly PveClient _client;
    private readonly ILog _log;
    private readonly ProxmoxAuthData _proxmoxData;
    private static ApiNetworkErrors PveNetworkErrors => ApiResultError.WithProxmox.ApiNetworkErrors;
    
    public Proxmox(PveClient client, ILog log, ProxmoxAuthData proxmoxData)
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
            var net = dnets is KeyValuePair<string, object> ? (KeyValuePair<string, object>)dnets : default;
            if (!net.Key.StartsWith("net")) continue;
            var netSettings = net.Value as string;

            // data ~= virtio=vaef,bridge=vmbr4.
            var data = netSettings.Split(",");
            var type = data[0].Split("=")[0];
            var iFace = data[1].Split('=')[1];

            result.Add(new NetSettings
            {
                Bridge = iFace,
                Model = type
            });
        }

        return result;
    }

    public async Task<Result> Create(string node, Net net)
    {
        var result = await _client.Nodes[node].Network
            .CreateNetwork(net.Bridge, net.Type, autostart: true);

        return result.Match(
            Result.Ok,
            reasonPhrases => PveNetworkErrors.Create(reasonPhrases, node, net.Bridge)
        );
    }


    public async Task<Result> Remove(string node, Net net)
    {
        var result = await _client.Nodes[node].Network[net.Bridge].DeleteNetwork();

        return result.Match(
            Result.Ok,
            responseError => ApiResultError.WithProxmox.NetworkDeleteFailure(responseError, node)
        );
    }

    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.Match(
            Result.Ok,
            reasonPhrases => PveNetworkErrors.Apply(reasonPhrases, node)
        );
    }


    public async Task<Result<List<int>>> GetAllQemu(string node)
    {
        var result = await _client.Nodes[node].Qemu.Vmlist(false);

        if (!result.IsSuccessStatusCode)
            return Result.Fail(ApiResultError.WithProxmox.Node.GetQuemies(node, result.ReasonPhrase));
        
        var vmids = new List<int>();
        var vmsData = result.Response.data;
        foreach (var vmData in vmsData)
        foreach (var vmElementData in vmData)
            if (vmElementData is KeyValuePair<string, object> { Key: "vmid", Value: long id })
                vmids.Add((int)id);

        vmids.Sort();
        return vmids;
    }

    public async Task<Result> Clone(CloneVmConfig vmConfig, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmConfig.TemplateData.Id].Clone.CloneVm(vmConfig.NewId);
        
        return result.Match(
            Result.Ok,
            ApiResultError.WithProxmox.CreateCloneFailure
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

    public async Task<Result<ProxmoxVmStatus>> GetStatus(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Current.VmStatus();
        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);

        var status = result.Response.data.qmpstatus as string;

        return Enum.Parse<ProxmoxVmStatus>(status.ToUpFirst());
    }

    public async Task<Result> Start(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            reasonPhrases => ApiResultError.WithProxmox.Vm.Start(reasonPhrases, node, qemu)
        );
    }

    public async Task<Result> Destroy(string node, int qemu)
    {
        var response = await _client.Nodes[node].Qemu[qemu].DestroyVm();
        
        return response.Match(
            Result.Ok,
            errors => ApiResultError.WithProxmox.Vm.Delete(node, qemu, errors)
        );
    }

    public async Task<Result> Stop(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Stop.VmStop();

        return result.Match(
            Result.Ok,
            rp => ApiResultError.WithProxmox.VmStopFailure(rp, node, qemu)
        );
    }


    public async Task<Result<Ip>> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail($"qemu {qemu} {result.ReasonPhrase}");

        // todo: декомпозиция. эта часть кода явно должн быть как минум в методе, а можно и в расширений, хотяя.
        // todo: честно я потом сделаю нормальный метод))))))
        var data = result.Response;
        var interfaces = data.data.result;

        foreach (var iFace in interfaces)
        foreach (var ipAddresses in iFace)
        {
            var ipAddressPair = ipAddresses is KeyValuePair<string, object>
                ? (KeyValuePair<string, object>)ipAddresses
                : default;
            if (ipAddressPair.Key != "ip-addresses") continue;
            var ipAddress = ipAddressPair.Value as List<dynamic>;
            foreach (var ipsD in ipAddress)
            foreach (var ipD in ipsD)
            {
                var ipPair = ipD is KeyValuePair<string, object> ? (KeyValuePair<string, object>)ipD : default;
                if (ipPair.Key != "ip-address") continue;

                var ip = ipPair.Value as string;
                if (!string.IsNullOrEmpty(ip) && ip.StartsWith(_proxmoxData.Ip.GetIdNetwork()))
                    return new Ip { Value = ipPair.Value as string };
            }
        }


        return Result.Fail("не нашлось");
    }
}