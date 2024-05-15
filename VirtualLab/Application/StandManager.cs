using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects;
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
    private readonly IProxmoxVm _proxmoxVm;
    private readonly IProxmoxNetwork _proxmoxNetworkDevice;
    private readonly ILog _log;

    public StandManager(IProxmoxVm proxmoxVm, IProxmoxNetwork proxmoxNetworkDevice, ILog log)
    {
        _proxmoxVm = proxmoxVm;
        _proxmoxNetworkDevice = proxmoxNetworkDevice;
        log.ForContext("LabVmManagment");
        _log = log;
    }


    public async Task<Result<IReadOnlyList<VirtualMachineInfo>>> Create(StandCreateConfig standCreateConfig)
    {
        var response = await CreateInterfaces(standCreateConfig.GetAllNetsInterfaces(), standCreateConfig.Node);
        if (response.IsFailed)
        {
            _log.Error($"create interface occured with errors: {response.Reasons}"); //todo: сделать так везде.
            return response;
        }

        response = await _proxmoxNetworkDevice.Apply(standCreateConfig.Node);
        if (response.IsFailed) return response;

        //todo: то есть до этого ты сделал метод, в котором foreach, а сейчас забил?
        foreach (var cloneVmTemplate in standCreateConfig.CloneVmConfig)
        {
            response = await CreateVmByTemplate(cloneVmTemplate, standCreateConfig.Node);
            if (response.IsFailed) return response;
        }

        //todo: максимально тупая реализация.

        var virtualMachineInfos = new List<VirtualMachineInfo>();

        foreach (var cloneVmConfig in standCreateConfig.CloneVmConfig)
        {
            if (!cloneVmConfig.Template.WithVmbr0)
            {
                virtualMachineInfos.Add(new VirtualMachineInfo()
                {
                    ProxmoxVmId = cloneVmConfig.NewId,
                    Password = cloneVmConfig.Template.Password,
                    Username = cloneVmConfig.Template.Name,
                    Node = standCreateConfig.Node
                });
            }
            else
            {
                var getIp = await _proxmoxVm.GetIp(standCreateConfig.Node, cloneVmConfig.NewId);
                if (getIp.IsFailed) return Result.Fail($"not found ip because: {getIp}");

                virtualMachineInfos.Add(new VirtualMachineInfo()
                {
                    ProxmoxVmId = cloneVmConfig.NewId,
                    Password = cloneVmConfig.Template.Password,
                    Username = cloneVmConfig.Template.Name,
                    Node = standCreateConfig.Node,
                    Ip = getIp.Value.IpV4
                });
            }
        }

        return virtualMachineInfos;
    }

    public async Task<Result> Delete(StandRemoveConfig standRemoveConfig)
    {
        var vmsDeleted = await DeleteVms(standRemoveConfig.VmsData.AsReadOnly());
        if (vmsDeleted.IsFailed) return vmsDeleted;
        

        foreach (var vmData in standRemoveConfig.VmsData.AsReadOnly())
        {
            foreach (var net in vmData.Nets)
            {
                var iFaceDeleted = await _proxmoxNetworkDevice.RemoveInterface(vmData.Node, net);
                if (iFaceDeleted.IsFailed) return iFaceDeleted;
            }
        }

        return Result.Ok();
    }

    private async Task<Result> DeleteVms(IReadOnlyCollection<VmInfo> vmsInfo)
    {
        foreach (var vmInfo in vmsInfo)
        {
            var vmStopped = await _proxmoxVm.StopVm(vmInfo.Node, vmInfo.ProxmoxId);
            if (vmStopped.IsFailed) return vmStopped;

            var getStatus = await _proxmoxVm.GetStatus(vmInfo.Node, vmInfo.ProxmoxId);
            if (!getStatus.TryGetValue(out var curVmStatus))
            {
                return Result.Fail(getStatus.Errors);
            }

            while (curVmStatus == ProxmoxVmStatus.Run)
            {
                Thread.Sleep(1000);
                getStatus = await _proxmoxVm.GetStatus(vmInfo.Node, vmInfo.ProxmoxId);
                if (getStatus.TryGetValue(out curVmStatus))
                {
                    return Result.Fail(getStatus.Errors);
                }
            }

            var vmDeleted = await _proxmoxVm.Delete(vmInfo.Node, vmInfo.ProxmoxId);
            if (vmDeleted.IsFailed) return vmDeleted;
        }

        return Result.Ok();
    }


    private async Task<Result> CreateInterfaces(IEnumerable<Net> nets, string node)
    {
        foreach (var net in nets.Where(x => x.Bridge != "vmbr0")) // чёт крижово выглядит.
        {
            var response = await _proxmoxNetworkDevice.CreateInterface(node, net);
            if (response.IsFailed) return Result.Fail($"net: {net} occured with error: {response}");
        }

        return Result.Ok();
    }

    // меня напрягает здесь node, как вообще будет работать node
    private async Task<Result> CreateVmByTemplate(CloneVmConfig cloneVmConfig, string node)
    {
        var response = await _proxmoxVm.Clone(cloneVmConfig, node);
        if (response.IsFailed) return response;
        // todo: refactoring in future
        response = await _proxmoxVm.UpdateDeviceInterface(new UpdateInterfaceForVm()
        {
            Node = node,
            Qemu = cloneVmConfig.NewId,
            Nets = cloneVmConfig.Nets
        });
        if (response.IsFailed) return response;

        response = await _proxmoxVm.StartVm(new LaunchVm() // а запускать мне кажется, точно должны не здесь.
        {
            Node = node,
            Qemu = cloneVmConfig.NewId
        });
        if (response.IsFailed) return response;

        return Result.Ok();
    }
}