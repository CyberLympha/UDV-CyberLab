using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

// название мне так же не очень нравятится. но суть в том, что это класс отвечает за создание лабы и предоставление её пользователю, кароче работает с proxomx и только.

public class StandManager : IStandManager
{
    private readonly ILog _log;
    private readonly IProxmoxNetwork _proxmoxNetworkDevice;
    private readonly IProxmoxVm _proxmoxVm;

    public StandManager(IProxmoxVm proxmoxVm, IProxmoxNetwork proxmoxNetworkDevice, ILog log)
    {
        _proxmoxVm = proxmoxVm;
        _proxmoxNetworkDevice = proxmoxNetworkDevice;
        log.ForContext("LabVmManagment");
        _log = log;
    }


    public async Task<Result<IReadOnlyList<VirtualMachineInfo>>> Create(StandCreateConfig standCreateConfig)
    {
        if ((await AddInterfaces(standCreateConfig.CloneVmConfig.GetAllNets(), standCreateConfig.Node))
            .IsFailedWithErrors(out var errors))
            Result.Fail(errors);

        //todo: то есть до этого ты сделал метод, в котором foreach, а сейчас забил?
        foreach (var cloneVmTemplate in standCreateConfig.CloneVmConfig)
        {
            var createdVm = await CreateVmByTemplate(cloneVmTemplate, standCreateConfig.Node);
            if (createdVm.IsFailed) return createdVm;
        }

        //todo: максимально тупая реализация.

        var virtualMachineInfos = new List<VirtualMachineInfo>();

        Thread.Sleep(55000); // ждём пока qemu agent запустится на машинах
        foreach (var cloneVmConfig in standCreateConfig.CloneVmConfig)
        {
            string ip = null!;
            if (cloneVmConfig.TemplateData.WithVmbr0)
            {
                var getIp = await _proxmoxVm.GetIp(standCreateConfig.Node, cloneVmConfig.NewId);
                if (!getIp.TryGetValue(out var ipData)) return Result.Fail($"not found ip because: {getIp}");

                ip = ipData.Value;
            }

            virtualMachineInfos.Add(new VirtualMachineInfo
            {
                ProxmoxVmId = cloneVmConfig.NewId,
                Password = cloneVmConfig.TemplateData.Password,
                Username = cloneVmConfig.TemplateData.Name,
                Node = standCreateConfig.Node,
                Ip = ip
            });
        }

        return virtualMachineInfos;
    }


    public async Task<Result> Delete(StandRemoveConfig standRemoveConfig)
    {
        var vmsDeleted = await DeleteVms(standRemoveConfig.VmsInfos.AsReadOnly());
        if (vmsDeleted.IsFailed) return vmsDeleted;

        await Interfaces(x =>
                _proxmoxNetworkDevice.Remove(
                    standRemoveConfig.VmsInfos[0].Node, x), // todo: кринж с array
            standRemoveConfig.Vms.GetAllNets());

        await _proxmoxNetworkDevice.Apply(standRemoveConfig.VmsInfos[0].Node); // тоже самое

        return Result.Ok();
    }


    private async Task<Result> AddInterfaces(IEnumerable<Net> nets, string node)
    {
        var response = await Interfaces(net => _proxmoxNetworkDevice.Create(node, net), nets);
        if (response.IsFailed)
        {
            _log.Error($"create interface occured with errors: {response.Reasons}"); //todo: сделать так везде.
            return response;
        }

        response = await _proxmoxNetworkDevice.Apply(node);
        if (response.IsFailed) return response;

        return Result.Ok();
    }


    private async Task<Result> DeleteVms(IReadOnlyCollection<VmInfo> vmsInfo) // здесь еще идёт стопа vm
    {
        foreach (var vmInfo in vmsInfo)
        {
            var vmStopped = await _proxmoxVm.Stop(vmInfo.Node, vmInfo.ProxmoxVmId);
            if (vmStopped.IsFailed) return vmStopped;

            var getStatus = await _proxmoxVm.GetStatus(vmInfo.Node, vmInfo.ProxmoxVmId);
            if (!getStatus.TryGetValue(out var curVmStatus)) return Result.Fail(getStatus.Errors);

            while (curVmStatus == ProxmoxVmStatus.Running)
            {
                Thread.Sleep(1000);
                getStatus = await _proxmoxVm.GetStatus(vmInfo.Node, vmInfo.ProxmoxVmId);
                if (!getStatus.TryGetValue(out curVmStatus)) return Result.Fail(getStatus.Errors);
            }

            var vmDeleted = await _proxmoxVm.Destroy(vmInfo.Node, vmInfo.ProxmoxVmId);
            if (vmDeleted.IsFailed) return vmDeleted;
        }

        return Result.Ok();
    }


    private async Task<Result> Interfaces(Func<Net, Task<Result>> action, IEnumerable<Net> nets)
    {
        foreach (var net in nets.WithoutVmbr0())
        {
            var response = await action(net);
            if (response.IsFailed) return Result.Fail($"net: {net} occured with error: {response}");
        }

        return Result.Ok();
    }

    // меня напрягает здесь node, как вообще будет работать node
    private async Task<Result> CreateVmByTemplate(CloneVmConfig cloneVmConfig, string node)
    {
        var response = await _proxmoxVm.Clone(cloneVmConfig, node);
        if (response.IsFailed) return response;

        if (cloneVmConfig.WithNets())
            response = await _proxmoxVm.UpdateDeviceInterface(node, cloneVmConfig.NewId, cloneVmConfig.Nets);
        if (response.IsFailed) return response;

        response = await _proxmoxVm.Start(node, cloneVmConfig.NewId); // а запускать мне кажется, точно должны не здесcm
        if (response.IsFailed) return response;

        return Result.Ok();
    }
}