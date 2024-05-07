using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Application;

public class Proxmox : IVmService, INetworkService // кажется в итоге это будет два отдельных класса)
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

    public async Task<Result> GetAllNetworksBridge(string node)
    {
        var result = await _client.Nodes[node].Network.Index("bridge");

        throw new NotImplementedException();
    }

    public async Task<Result> CreateInterface(CreateInterface request)
    {
        var result = await _client.Nodes[request.Node].Network.CreateNetwork(request.IFace, request.Type, autostart: true);

        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.NetworkCreateError(reasonPhrases, request.Node),
            errors => ApiResultError.WithProxmox.NetworkCreateError(errors, request.Node)
        );
    }

    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.WithProxmox.NetworkApplyError(reasonPhrases, node),
            errors => ApiResultError.WithProxmox.NetworkApplyError(errors, node)
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

    public async Task<Result> StartVm(LaunchVm request)
    {
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            reasonPhrases => ApiResultError.WithProxmox.VmStartFailure(reasonPhrases, request),
            errors => ApiResultError.WithProxmox.VmStartFailure(errors, request)
        );
    }


    public async Task<Result<Ip>> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);

        // todo: декомпозиция. эта часть кода явно должн быть как минум в методе, а можно и в расширений, хотяя.
        // todo: и вообще не здесь
        var interfaces = result.Response;
        
        foreach (var @interface in interfaces)
        {
            if (interfaces["name"] == "vmbr0")
            {
                var ip = interfaces["ip-addresses"]["ip-address"] as string;
                return new Ip() { IpV4 = ip };
            }
        }
        //todo: доделать

        return Result.Fail("не нашлось");
    }
}