using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox.Requests;
using Vostok.Logging.Abstractions;
using Guid = System.Guid;

namespace VirtualLab.Application;

// название мне так же не очень нравятится. но суть в том, что это класс отвечает за создание лабы и предоставление её пользователю, кароче работает с proxomx и только.

public class LabVirtualMachineManager : ILabVirtualMachineManager
{
    private readonly IVmService _vm;
    private readonly INetworkService _networkDevice;
    private readonly ILog _log;

    public LabVirtualMachineManager(IVmService vm, INetworkService networkDevice, ILog log)
    {
        _vm = vm;
        _networkDevice = networkDevice;
        log.ForContext("LabVmManagment");
        _log = log;
    }


    public async Task<Result<IReadOnlyList<LabEntryPoint>>> CreateLab(LabConfig labConfig)
    {
        var response = await CreateInterfaces(labConfig.GetAllNetsInterfaces(), labConfig.Node);
        if (response.IsFailed)
        {
            _log.Error($"create interface occured with errors: {response.Reasons}"); //todo: сделать так везде.
            return response;
        }

        response = await _networkDevice.Apply(labConfig.Node);
        if (response.IsFailed) return response;

        //todo: то есть до этого ты сделал метод, в котором foreach, а сейчас забил?
        foreach (var cloneVmTemplate in labConfig.CloneVmConfig)
        {
            response = await CreateVmByTemplate(cloneVmTemplate, labConfig.Node);
            if (response.IsFailed) return response;
        }

        //todo: максимально тупая реализация.

        var entryPoints = new List<LabEntryPoint>();
        foreach (var template in labConfig.CloneVmConfig)
        {
            if (!template.Template.WithVmbr0) continue;

            var GetIp = await _vm.GetIp(labConfig.Node, template.NewId);
            if (GetIp.IsFailed) return response;
            // пока масксимально просто для первых тестов.
            entryPoints.Add(LabEntryPoint.From(
                GetIp.Value.IpV4, 
                template.Template.Name, 
                template.Template.Password,
                labConfig.LabId)
            );
        }

        if (entryPoints.Count == 0) return Result.Fail("а как то нету открытых портов");
        
        return entryPoints;
    }

    public Task<Result<IReadOnlyList<LabEntryPoint>>> GetEntryPoint(Guid labId)
    {
        throw new NotImplementedException();
    }

    private async Task<Result> CreateInterfaces(IEnumerable<Net> nets, string node)
    {
        foreach (var net in nets)
        {
            var response = await _networkDevice.CreateInterface(CreateInterface.Brige(net.Bridge, node));
            if (response.IsFailed) return response;
        }

        return Result.Ok();
    }

    // меня напрягает здесь node, как вообще будет работать node
    private async Task<Result> CreateVmByTemplate(CloneVmConfig cloneVmConfig, string node)
    {
        var response = await _vm.Clone(cloneVmConfig, node);
        if (response.IsFailed) return response;
        // todo: refactoring in future
        response = await _vm.UpdateDeviceInterface(new UpdateInterfaceForVm()
        {
            Node = node,
            Qemu = cloneVmConfig.NewId,
            Nets = cloneVmConfig.Nets
        });
        if (response.IsFailed) return response;

        response = await _vm.StartVm(new LaunchVm()
        {
            Node = node,
            Qemu = cloneVmConfig.NewId
        });
        if (response.IsFailed) return response;

        return Result.Ok();
    }
}