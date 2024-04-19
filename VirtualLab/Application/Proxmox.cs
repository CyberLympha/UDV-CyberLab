using Corsinvest.ProxmoxVE.Api;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Infrastructure;
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

    // сначала клонируем
    public async Task<Result> Clone(CloneVmTemplate vmTemplate, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmTemplate.VmIdTemplate].Clone.CloneVm(vmTemplate.NewId);

        return result.ResponseInError
            ? Result.Fail(ApiResultError.GenerateTemplateCloneFailure(result.GetError()))
            : Result.Ok();
    }

    // возможно будет 2
    public Task<Result> GetAllNetworks(string node)
    {
        throw new NotImplementedException();
    }

    // создаём новый интерйес 3
    public async Task<Result> Create(CreateInterface request)
    {
        var result = await _client.Nodes[request.Node].Network.CreateNetwork(request.IFace, request.Type);
        _log.Info($"inteface created {request}");
        
        return result.ResponseInError
            ? Result.Fail(ApiResultError.NetworkCreateError(result.GetError(), request.Node))
            : Result.Ok();
    }

    // подтвержаем его 4
    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.ResponseInError
            ? Result.Fail(ApiResultError.NetworkApplyError(result.GetError(), node))
            : Result.Ok();
    }

    // меняем интефейс у клонов templeta на новый интерфейс. 5
    public async Task<Result> ChangeDeviceInterface(ChangeInterfaceForVm request)
    {
        var nets = new Dictionary<int, string>(request.Nets);
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Config.UpdateVmAsync(netN: nets);

        return result.ResponseInError
            ? Result.Fail(ApiResultError.ChangeIntefaceFailure(result.GetError(), request))
            : Result.Ok();
    }


    // запускаем эти машины 6
    public async Task<Result> StartVm(LaunchVm request)
    {
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Status.Start.VmStart();

        return result.ResponseInError
            ? Result.Fail(ApiResultError.VmStartFailure(result.GetError(), request))
            : Result.Ok();
    }


    public Task<Result> GetIp(long id)
    {
        throw new NotImplementedException();
    }
}