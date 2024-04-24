using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox.Requests;
using VirtualLab.Infrastructure;
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

    public async Task<Result> Clone(CloneRequest request, string node)
    {
        var result = await _client.Nodes[node].Qemu[request.Template.Id].Clone.CloneVm(request.NewId);


        return result.Match(
            Result.Ok,
            ApiResultError.GenerateTemplateCloneFailure,
            ApiResultError.GenerateTemplateCloneFailure);
    }

    public Task<Result> GetAllNetworks(string node)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateInterface(CreateInterface request)
    {
        var result = await _client.Nodes[request.Node].Network.CreateNetwork(request.IFace, request.Type);
        
        return result.Match(
            Result.Ok,
            reasonPhrases => ApiResultError.NetworkCreateError(reasonPhrases, request.Node),
            errors => ApiResultError.NetworkCreateError(errors, request.Node)
        );
    }

    public async Task<Result> Apply(string node)
    {
        var result = await _client.Nodes[node].Network.ReloadNetworkConfig();

        return result.Match(
            Result.Ok, 
            reasonPhrases => ApiResultError.NetworkApplyError(reasonPhrases, node),
            errors => ApiResultError.NetworkApplyError(errors, node)
        );
    }

    public async Task<Result> UpdateDeviceInterface(UpdateInterfaceForVm request)
    {
        var nets = new Dictionary<int, string>(request.Nets.Value);
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Config.UpdateVmAsync(netN: nets);

       
        return result.Match(
            Result.Ok, 
            reasonPhrases => ApiResultError.ChangeIntefaceFailure(reasonPhrases, request),
            errors => ApiResultError.ChangeIntefaceFailure(errors, request)
        );
    }
    
    public async Task<Result> StartVm(LaunchVm request)
    {
        var result = await _client.Nodes[request.Node].Qemu[request.Qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            reasonPhrases => ApiResultError.VmStartFailure(reasonPhrases, request), 
            errors => ApiResultError.VmStartFailure(errors, request)
        );
    }



    public async Task<Result> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);
        var interfaces = result.Response[0]["name"];
        //todo: доделать

        throw new NotImplementedException();
    }
}