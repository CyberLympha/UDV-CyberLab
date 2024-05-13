using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using Vostok.Logging.Abstractions;
using Guid = System.Guid;

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


    public async Task<Result<IReadOnlyList<VirtualMachineInfo>>> CreateStand(StandCreateConfig standCreateConfig)
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
                if (getIp.IsFailed) return response;
                
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

    public Task<Result> RemoveStand(StandRemoveConfig standRemoveConfig)
    {
        
    }

  
    private async Task<Result> CreateInterfaces(IEnumerable<Net> nets, string node)
    {
        foreach (var net in nets.Where(x => x.Bridge != "vmbr0"))
        {
            var response = await _proxmoxNetworkDevice.CreateInterface(CreateInterface.Brige(net.Bridge, node));
            if (response.IsFailed) return response;
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