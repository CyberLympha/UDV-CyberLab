using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabConfigure : ILabConfigure
{
    private readonly ILabRepository _labs;
    private readonly ILog _log;
    private readonly IVirtualMachineDataHandler _virtualMachines;
    private readonly IProxmoxNetwork _proxmoxNetwork;

    public LabConfigure(ILabRepository labs,
        ILog log,
        IVirtualMachineDataHandler virtualMachines,
        IProxmoxNetwork proxmoxNetwork)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
        _virtualMachines = virtualMachines;
        _proxmoxNetwork = proxmoxNetwork;
    }


    public async Task<Result<StandCreateConfig>> GetConfigByLab(Guid labId)
    {
        var getTemplateConfig = await GetTemplateConfig(labId);
        if (getTemplateConfig.IsFailed) return Result.Fail(getTemplateConfig.Errors);

        var getConfigCurLab = await GenerateLabConfig(getTemplateConfig.Value);
        if (getConfigCurLab.IsFailed) return Result.Fail(getConfigCurLab.Errors);

        return getConfigCurLab;
    }

    public async Task<Result<StandRemoveConfig>> GetConfigByUserLab(Guid userLabId)
    {
        var resultVms = await _virtualMachines.GetAllByUserLabId(userLabId);
        if (!resultVms.TryGetValue(out var virtualMachines))
        {
            return Result.Fail($"ошибка: {resultVms.Errors}");
        }


        var userLabConfig = new StandRemoveConfig();
        foreach (var virtualMachine in virtualMachines)
        {
            var resultNets = await _proxmoxNetwork.
                GetAllNetworksBridgeByVm(virtualMachine.ProxmoxVmId, virtualMachine.Node);
            if (!resultNets.TryGetValue(out var nets))
            {
                return Result.Fail($"error: {resultNets.Errors}");
            }
            
            userLabConfig.VmsData.Add(new VmInfo
            {
                ProxmoxId = virtualMachine.ProxmoxVmId,
                Nets = nets
            });
        }

        return userLabConfig;
    }

    //todo: псс, mongoDb норм идея, для хранием конфигов лаб)) по идей, могут быть доп данные.
    private async Task<Result<StandCreateConfig>> GetTemplateConfig(Guid labId)
    {
        var lab = await _labs.Get(labId);
        if (lab.IsFailed)
        {
            return Result.Fail(lab.Errors);
        }

        var net24 = new NetSettings()
        {
            Bridge = "vmbr24",
            Model = "virtio"
        };
        
        var vmbr0 = new NetSettings()
        {
            Bridge = "vmbr0",
            Model = "virtio"
        };

        var nets1300vm = new NetCollection { net24, vmbr0 };
        var nets1301Vm = new NetCollection { net24 };
        var nets1302Vm = new NetCollection() { net24 };
        
        var labConfig = new StandCreateConfig()
        {
            Node = "pve",
            CloneVmConfig = new List<CloneVmConfig>()
            {
                new()
                {
                    Template = new Template() //kali
                    {
                        WithVmbr0 = true,
                        Id = 300,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 1300,
                    Nets = nets1300vm
                },
                /*new()
                {
                    Template = new Template() // win
                    {
                        WithVmbr0 = false,
                        Id = 301,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 1301,
                    Nets = nets1301Vm
                },
                new()
                {
                    Template = new Template() // win
                    {
                        WithVmbr0 = false,
                        Id = 302,
                        Password = "test",
                        Name = "test"
                    },
                    NewId = 1302,
                    Nets = nets1302Vm
                }*/
            }
        };

        _log.Info($"configure created {labConfig}");
        return labConfig;
    }

    private async Task<Result<StandCreateConfig>> GenerateLabConfig(StandCreateConfig standCreateConfig)
    {
        return standCreateConfig;
    }
}