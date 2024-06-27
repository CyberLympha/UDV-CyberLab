using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;
using Result = FluentResults.Result;

namespace VirtualLab.Infrastructure.Pve;

public class PveVm : IProxmoxVm
{
    private readonly PveClient _client;
    private readonly ProxmoxAuthData _proxmoxData;
    private static VmApiResult ErrorWhen => ApiResultError.WithProxmox.Vm;

    public PveVm(PveClient client, ProxmoxAuthData proxmoxData)
    {
        _client = client;
        _proxmoxData = proxmoxData;
    }

    public async Task<Result> Clone(CloneVmConfig vmConfig, string node)
    {
        var result = await _client.Nodes[node].Qemu[vmConfig.TemplateData.Id].Clone.CloneVm(vmConfig.NewId);

        return result.Match(
            Result.Ok,
            ErrorWhen.CreateClone
        );
    }

    public async Task<Result> UpdateDeviceInterface(string node, int qemu, NetCollection nets)
    {
        var netN = new Dictionary<int, string>(nets.Value);
        var result = await _client.Nodes[node].Qemu[qemu].Config.UpdateVmAsync(netN: netN);


        return result.Match(
            Result.Ok,
            errors => ApiResultError.WithProxmox.ChangeInterfaceFailure(errors, node, qemu)
        );
    }

    public async Task<Result<ProxmoxVmStatus>> GetStatus(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Current.VmStatus();
        if (!result.IsSuccessStatusCode) return result.Fail(errors => 
            ErrorWhen.VmGetStatusFailure(errors, node, qemu));

        var status = result.Response.data.qmpstatus as string;

        return Enum.Parse<ProxmoxVmStatus>(status.ToUpFirst());
    }

    public async Task<Result> Start(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Start.VmStart();

        return result.Match(
            Result.Ok, // todo возможно, что данные в какой node ошибка прописываеется в ответе от proxmox.
            errors => ErrorWhen.Start(errors, node, qemu)
        );
    }

    public async Task<Result> Destroy(string node, int qemu)
    {
        var response = await _client.Nodes[node].Qemu[qemu].DestroyVm();

        return response.Match(
            Result.Ok,
            errors => ErrorWhen.Delete(node, qemu, errors)
        );
    }

    public async Task<Result> Stop(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Status.Stop.VmStop();

        return result.Match(
            Result.Ok,
            errors => ErrorWhen.VmStop(errors, node, qemu)
        );
    }

    public async Task<Result<Ip>> GetIp(string node, int qemu)
    {
        var result = await _client.Nodes[node].Qemu[qemu].Agent.NetworkGetInterfaces.NetworkGetInterfaces();

        if (!result.IsSuccessStatusCode) return Result.Fail(result.ReasonPhrase);

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