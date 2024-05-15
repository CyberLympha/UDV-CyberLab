using System.Net.NetworkInformation;
using System.Text.Json;
using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects;
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
    private ILog _log;

    public Proxmox(PveClient client, ILog log)
    {
        _client = client;
        _log = log;
    }

    public async Task<Result> Clone(CloneVmConfig vmConfig, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmConfig.Template.Id].Clone.CloneVm(vmConfig.NewId);


        return result.Match(
            Result.Ok,
            ApiResultError.WithProxmox.CreateCloneFailure,
            ApiResultError.WithProxmox.CreateCloneFailure);
    }

    public async Task<Result> GetAllNetworksBridgeByVm(string node)
    {
        var result = await _client.Nodes[node].Network.Index("bridge");

        throw new NotImplementedException();
    }

    public async Task<Result<NetCollection>> GetAllNetworksBridgeByVm(int vmId, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmId].Config.VmConfig();

        var listNet = result.Response["data"]["config"]["net"];
        throw new NotImplementedException();
    }

    public async Task<Result> CreateInterface(string node , Net net)
    {
        var result = await _client.Nodes[node].Network
            .CreateNetwork(net.Bridge, net.Type, autostart: true);

        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.Network.Create(reasonPhrases, node, net.Bridge),
            errors => ApiResultError.WithProxmox.Network.Create(errors, node, net.Bridge)
        );
    }


    public async Task<Result> RemoveInterface(string node, Net net)
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

    public async Task<Result> UpdateDeviceInterface(UpdateInterfaceForVm request)
    {
        var nets = new Dictionary<int, string>(request.Nets.Value);
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Config.UpdateVmAsync(netN: nets);


        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.ChangeIntefaceFailure(reasonPhrases, request),
            errors => ApiResultError.WithProxmox.ChangeIntefaceFailure(errors, request)
        );
    }

    public async Task<Result<ProxmoxVmStatus>> GetStatus(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Current.VmStatus();

        throw new NotImplementedException();
        return result.Match(
            Result.Ok,
            responseError => ApiResultError.WithProxmox.VmGetStatusFailure(responseError, node, qemu),
        errors => ApiResultError.WithProxmox.VmGetStatusFailure(errors, node, qemu));
    }

    public async Task<Result> StartVm(LaunchVm request)
    {
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            reasonPhrases => ApiResultError.WithProxmox.VmStartFailure(reasonPhrases, request),
            errors => ApiResultError.WithProxmox.VmStartFailure(errors, request)
        );
    }

    public Task<Result> Delete(string node, int qemu)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> StopVm(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Stop.VmStop();

        return result.Match(
            Result.Ok,
            rp => ApiResultError.WithProxmox.VmStopFailure(rp, node, qemu),
             errors => ApiResultError.WithProxmox.VmStopFailure(errors, node,qemu));
    }


    public async Task<Result<Ip>> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);

        // todo: декомпозиция. эта часть кода явно должн быть как минум в методе, а можно и в расширений, хотяя.
        // todo: и вообще не здесь
        var data = result.Response;
        var interfaces = data.data.result;

        foreach (var @interface in interfaces)
        {
            var ipAddresses = @interface["ip-addresses"];
            foreach (var ipAddress in ipAddresses)
            {
                var ip = ipAddress["ip-address"] as string;
                _log.Debug($"{ip}");
                if (!string.IsNullOrEmpty(ip) && ip.StartsWith(ProxmoxData.NetworkIdGlobalNetwork))
                {
                    _log.Info($"ip {ip} from {node} & {qemu}");
                    return new Ip() { IpV4 = ip };
                }
            }
        }
        //todo: доделать

        return Result.Fail("не нашлось");
    }
}