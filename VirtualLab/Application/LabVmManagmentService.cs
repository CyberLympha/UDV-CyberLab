using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Application;

// название мне так же не очень нравятится. но суть в том, что это класс отвечает за создание лабы и предоставление её пользователю, кароче работает с proxomx и только.

public class LabVmManagementService : ILabVmManagementService {
    private readonly IVmService _vm;
    private readonly INetworkService _networkDevice;

    public LabVmManagementService(IVmService vm, INetworkService networkDevice)
    {
        _vm = vm;
        _networkDevice = networkDevice;
    }


    public async Task<Result<LabEntryPoint>> CreateLab(LabNodeConfig labNodeConfig)
    {
        await _networkDevice.Create(new CreateInterface());
        await _networkDevice.Apply(labNodeConfig.Node);
     
        foreach (var cloneVmTemplate in labNodeConfig.CloneVmTemplates)
        {
            await CreateVmByTemplate(cloneVmTemplate, labNodeConfig.Nets, labNodeConfig.Node);
        }
        
        //_vm.GetIp();

        throw new NotImplementedException();
    }

    // меня напрягает здесь node, как вообще будет работать node
    private async Task CreateVmByTemplate(CloneVmTemplate cloneVmTemplate, NetCollection nets, string node)
    {
        await _vm.Clone(cloneVmTemplate, node);

        await _vm.ChangeDeviceInterface(new ChangeInterfaceForVm()
        {
            Node = node,
            Qemu = cloneVmTemplate.NewId,
            Nets = nets
        });


        await _vm.StartVm(new LaunchVm()
        {
            Node = node,
            Qemu = cloneVmTemplate.NewId
        });
    }
    
    // просто есть. метод бесполезный и не нужный.
    public Task<Result<LabEntryPoint>> CreateVmWithLab(CloneVmTemplate cloneVmTemplate, string node)
    {
        _vm.Clone(cloneVmTemplate, node);

        //_network.CreateNetworkDevice();
        _networkDevice.Apply(node);

        //_vm.ChangeInterface();


        _vm.StartVm(new LaunchVm()
        {
            Node = node, Qemu = cloneVmTemplate.NewId
        }); // todo: можно заюзать implicate метод в LaunchVm.

        //_vm.GetIp()

        throw new NotImplementedException();
    }
}