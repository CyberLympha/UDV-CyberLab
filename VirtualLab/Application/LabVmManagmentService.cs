using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox.Requests;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

// название мне так же не очень нравятится. но суть в том, что это класс отвечает за создание лабы и предоставление её пользователю, кароче работает с proxomx и только.

public class LabVmManagementService : ILabVmManagementService
{
    private readonly IVmService _vm;
    private readonly INetworkService _networkDevice;
    private readonly ILog _log;

    public LabVmManagementService(IVmService vm, INetworkService networkDevice, ILog log)
    {
        _vm = vm;
        _networkDevice = networkDevice;
        log.ForContext("LabVmManagment");
        _log = log;
    }


    public async Task<Result<LabEntryPoint>> CreateLab(LabCreateRequest labCreateRequest)
    {
        var response = await CreateInterfaces(labCreateRequest);
        if (response.IsFailed)
        {
            _log.Error($"create interface occured with errors:{response.Reasons}"); //todo: сделать так везде.
            return response;
        }


        response = await _networkDevice.Apply(labCreateRequest.Node);
        if (response.IsFailed) return response;

        //todo: то есть до этого ты сделал метод, в котором foreach, а сейчас забил?
        foreach (var cloneVmTemplate in labCreateRequest.ClonesRequest)
        {
            response = await CreateVmByTemplate(cloneVmTemplate, labCreateRequest.Nets, labCreateRequest.Node);
            if (response.IsFailed) return response;
        }

        //todo: максимально тупая реализация.
        foreach (var template in labCreateRequest.ClonesRequest)
        {
            if (!template.Template.WithVmbr0) continue;


            var responseIp = await _vm.GetIp(labCreateRequest.Node, template.NewId);
            if (responseIp.IsFailed) return response;
            // пока масксимально просто для первых тестов.
            return new LabEntryPoint()
            {
                Ip = responseIp.Value.IpV4,
                Name = template.Template.Name,
                Password = template.Template.Password
            };
        }


        throw new NotImplementedException();
    }

    private async Task<Result> CreateInterfaces(LabCreateRequest labCreateRequest)
    {
        foreach (var net in labCreateRequest.Nets)
        {
            var response = await _networkDevice.CreateInterface(new CreateInterface
            {
                Node = labCreateRequest.Node,
                Type = "bridge",
                IFace = net.Bridge
            });
            if (response.IsFailed) return response;
        }

        return Result.Ok();
    }

    // меня напрягает здесь node, как вообще будет работать node
    private async Task<Result> CreateVmByTemplate(CloneRequest cloneRequest, NetCollection nets, string node)
    {
        var response = await _vm.Clone(cloneRequest, node);
        if (response.IsFailed) return response;
        // todo: refactoring in future
        response = await _vm.UpdateDeviceInterface(new UpdateInterfaceForVm()
        {
            Node = node,
            Qemu = cloneRequest.NewId,
            Nets = nets
        });
        if (response.IsFailed) return response;

        response = await _vm.StartVm(new LaunchVm()
        {
            Node = node,
            Qemu = cloneRequest.NewId
        });
        if (response.IsFailed) return response;

        return Result.Ok();
    }
}