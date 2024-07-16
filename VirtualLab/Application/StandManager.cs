using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
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


    public async Task<Result<IReadOnlyList<NewVmInfo>>> Create(StandCreateConfig standCreateConfig)
    {
        var added = await AddNewInterfaces(standCreateConfig.GetAllNets(), standCreateConfig.Node);
        if (added.IsFailed) return added;
        
        var result = await ExecuteStandAction(
            standCreateConfig.CloneVmConfig.ToArray, // сомнительная штука ну ладно.
            config => DeployNewVm(config, standCreateConfig.Node));
        if (result.IsFailed) return result;

        Thread.Sleep(55000); // ждём пока qemu agent запустится на машинах -- по идей лучше нам смотреть, каждый 10 секунд, а запущен ли agent
        if (!(await GetVmsInfo(standCreateConfig)).TryGetValue(out var vmsInfo, out var errors))
        {
            return Result.Fail(errors);
        }

        return vmsInfo;
    }

    public async Task<Result> Delete(StandRemoveConfig standRemoveConfig)
    {
        var vmsDeleted = await DeleteVms(standRemoveConfig.VmsInfos.AsReadOnly());
        if (vmsDeleted.IsFailed) return vmsDeleted;

        var result = await ExecuteStandAction(
            standRemoveConfig.Vms.GetAllNets().WithoutVmbr0,
            x => _proxmoxNetworkDevice.Remove(standRemoveConfig.VmsInfos[0].Node, x) // todo: кринж с array
        );
        if (result.IsFailed) return result;
        
        await _proxmoxNetworkDevice.Apply(standRemoveConfig.VmsInfos[0].Node); // тоже самое
        
        return Result.Ok();
    }

    private async Task<Result<List<NewVmInfo>>> GetVmsInfo(StandCreateConfig standCreateConfig)
    {
        var virtualMachineInfos = new List<NewVmInfo>();
        foreach (var cloneVmConfig in standCreateConfig.CloneVmConfig)
        {
            var vminfo = NewVmInfo.From(cloneVmConfig);
            if (cloneVmConfig.TemplateData.Nets.HaveVmbr0())
            {
                var getIp = await _proxmoxVm.GetIp(standCreateConfig.Node, cloneVmConfig.newQemu.Id);
                if (!getIp.TryGetValue(out var ipData))
                {
                    return Result.Fail($"not found ip because: {getIp}");
                }

                vminfo.Ip = ipData.Value;
            }

            virtualMachineInfos.Add(vminfo);
        }

        return virtualMachineInfos;
    }


    private async Task<Result> AddNewInterfaces(IEnumerable<Net> nets, string node)
    {
        var response = await ExecuteStandAction(nets.WithoutVmbr0, net => _proxmoxNetworkDevice.Create(node, net));
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

    /*private async Task<Result> Interfaces(IEnumerable<Net> nets, Func<Net, Task<Result>> action)
    {
        foreach (var net in nets.WithoutVmbr0())
        {
            var response = await action(net);
            if (response.IsFailed) return Result.Fail($"net: {net} occured with error: {response}");
        }

        return Result.Ok();
    }*/ //todo если всё работает можно убрать эту штуку.
    private static async Task<Result> ExecuteStandAction<T>(Func<IEnumerable<T>> elements, Func<T, Task<Result>> action)
    {
        foreach (var element in elements())
        {
            var response = await action(element);
            if (response.IsFailed) return Result.Fail($"error with {typeof(T).Name} occured with error: {response}");
        }

        return Result.Ok();
    }

    private async Task<Result> DeployNewVm(CloneVmConfig cloneVmConfig, string node)
    {
        var response = await _proxmoxVm.Clone(node, cloneVmConfig.newQemu.Id, cloneVmConfig.TemplateData.Id);
        if (response.IsFailed) return response;

        if (cloneVmConfig.TemplateData.WithNets())
            response = await _proxmoxVm.UpdateDeviceInterface(node, cloneVmConfig.newQemu.Id,
                cloneVmConfig.TemplateData.Nets);
        if (response.IsFailed) return response;

        response = await _proxmoxVm.Start(node,
            cloneVmConfig.newQemu.Id); // а запускать мне кажется, точно должны не здесcm
        if (response.IsFailed) return response;

        return Result.Ok();
    }
}