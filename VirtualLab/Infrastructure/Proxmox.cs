using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Application;

public class Proxmox : IProxmoxVm, IProxmoxNetwork // кажется в итоге это будет два отдельных класса)
{
    private readonly PveClient _client;
    private readonly ILog _log;
    private readonly ProxmoxAuthData proxmoxData;
    public Proxmox(PveClient client, ILog log, ProxmoxAuthData proxmoxData)
    {
        _client = client;
        _log = log;
        proxmoxData = proxmoxData;

    }

    public async Task<Result> Clone(CloneVmConfig vmConfig, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmConfig.Template.Id].Clone.CloneVm(vmConfig.NewId);


        return result.Match(
            Result.Ok,
            ApiResultError.WithProxmox.CreateCloneFailure,
            ApiResultError.WithProxmox.CreateCloneFailure);
    }


    public async Task<Result<NetCollection>> GetAllNetworksBridgeByVm(int vmId, string node)
    {
        var response = await _client.Nodes[node].Qemu[vmId].Config.VmConfig();

        var listNet = response.Response.data;

        var result = new NetCollection();

        // здесь ужасная реализация
        foreach (var Dnets in listNet)
        {
            var net = (Dnets is KeyValuePair<string, object> ? (KeyValuePair<string, object>)Dnets : default);
            if (!net.Key.StartsWith("net")) continue;
            var netSettings = net.Value as string;
            
            // что-то страшное ответ : virtio=vaef,bridge=vmbr4.
            var data = netSettings.Split(",");
            var type = data[0].Split("=")[0];
            var iFace = data[1].Split('=')[1];

            result.Add(new NetSettings()
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
            reasonPhrases => ApiResultError.WithProxmox.Network.Create(reasonPhrases, node, net.Bridge),
            errors => ApiResultError.WithProxmox.Network.Create(errors, node, net.Bridge)
        );
    }


    public async Task<Result> Remove(string node, Net net)
    {
        var result = await _client.Nodes[node].Network[net.Bridge].DeleteNetwork();

        return result.Match(
            Result.Ok,
            responseError => ApiResultError.WithProxmox.NetworkDeleteFailure(responseError, node),
            erros => ApiResultError.WithProxmox.NetworkDeleteFailure(erros, node)
        );
    }

    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.Network.Apply(reasonPhrases, node),
            errors => ApiResultError.WithProxmox.Network.Apply(errors, node)
        );
    }

    public async Task<Result> UpdateDeviceInterface(string node, int qemu, NetCollection nets)
    {
        var netN = new Dictionary<int, string>(nets.Value);
        var result = await _client.Nodes[node].Qemu[qemu].Config.UpdateVmAsync(netN: netN);


        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.ChangeInterfaceFailure(reasonPhrases, node,qemu),
            errors => ApiResultError.WithProxmox.ChangeInterfaceFailure(errors, node,qemu)
        );
    }

    public async Task<Result<ProxmoxVmStatus>> GetStatus(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Current.VmStatus();
        if (!result.IsSuccessStatusCode)
        {
            return Result.Fail(result.ReasonPhrase);
        }

        var status = result.Response.data.qmpstatus as string;

        return Enum.Parse<ProxmoxVmStatus>(status.ToUpFirst());
    }

    public async Task<Result> Start(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            reasonPhrases => ApiResultError.WithProxmox.Vm.Start(reasonPhrases, node, qemu),
            errors => ApiResultError.WithProxmox.Vm.Start(errors, node, qemu)
        );
    }

    public async Task<Result> Destroy(string node, int qemu)
    {
        var response = await _client.Nodes[node].Qemu[qemu].DestroyVm();

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail(response.ReasonPhrase);
        }

        return response.Match(
            Result.Ok,
            responseError => ApiResultError.WithProxmox.Vm.Delete(node, qemu, responseError),
            errors => ApiResultError.WithProxmox.Vm.Delete(node, qemu, errors));
    }

    public async Task<Result> Stop(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Stop.VmStop();

        return result.Match(
            Result.Ok,
            rp => ApiResultError.WithProxmox.VmStopFailure(rp, node, qemu),
            errors => ApiResultError.WithProxmox.VmStopFailure(errors, node, qemu));
    }


    public async Task<Result<Ip>> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);

        // todo: декомпозиция. эта часть кода явно должн быть как минум в методе, а можно и в расширений, хотяя.
        // todo: и вообще не здесь
        var data = result.Response;
        var interfaces = data.data.result;

        foreach (var iFace in interfaces)
        {
            foreach (var ipAddresses in iFace)
            {
                var ipAddressPair = ipAddresses is KeyValuePair<string, object>
                    ? (KeyValuePair<string, object>)ipAddresses
                    : default;
                if (ipAddressPair.Key != "ip-addresses") continue;
                var ipAddress = ipAddressPair.Value as List<dynamic>;
                foreach (var ipsD in ipAddress)
                {
                    foreach (var ipD in ipsD)
                    {
                        var ipPair = ipD is KeyValuePair<string, object> ? (KeyValuePair<string, object>)ipD : default;
                        if (ipPair.Key != "ip-address") continue;

                        var ip = ipPair.Value as string;
                        if (!string.IsNullOrEmpty(ip) && ip.StartsWith(proxmoxData.Ip.GetIdNetwork()))
                        {
                            return new Ip() { Value = ipPair.Value as string };
                        }
                    }
                }
            }
        }


        return Result.Fail("не нашлось");
    }
}