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
    private readonly ICredentialRepository _credentials;
    private readonly IVirtualMachineRepository _virtualMachines;
    private readonly IProxmoxNetwork _proxmoxNetwork;

    public LabConfigure(ILabRepository labs,
        ILog log,
        ICredentialRepository credentials,
        IVirtualMachineRepository virtualMachines, IProxmoxNetwork proxmoxNetwork)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
        _credentials = credentials;
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
        var resultVms = await _virtualMachines.GetAllByUserLab(userLabId);
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
            
            userLabConfig.VmsData.Add(new VmData
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

        var net2005 = new NetSettings()
        {
            Bridge = "vmbr2005",
            Model = "virtio"
        };
        var net2006 = new NetSettings()
        {
            Bridge = "vmbr2006",
            Model = "virtio"
        };
        var net2007 = new NetSettings()
        {
            Bridge = "vmbr2007",
            Model = "virtio"
        };

        var vmbr0 = new NetSettings()
        {
            Bridge = "vmbr0",
            Model = "virio"
        };

        var nets1005vm = new NetCollection { net2005 };
        var nets1006Vm = new NetCollection { net2006 };
        var nets1007Vm = new NetCollection() { net2007 };
        var router = new NetCollection() { net2005, net2006, net2007 };


        var labConfig = new StandCreateConfig()
        {
            Node = "test",
            CloneVmConfig = new List<CloneVmConfig>()
            {
                new()
                {
                    Template = new Template() //kali
                    {
                        WithVmbr0 = false,
                        Id = 1005,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 2005,
                    Nets = nets1005vm
                },
                new()
                {
                    Template = new Template() // win
                    {
                        WithVmbr0 = false,
                        Id = 1006,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 2006,
                    Nets = nets1006Vm
                },
                new()
                {
                    Template = new Template() // win
                    {
                        WithVmbr0 = false,
                        Id = 1007,
                        Password = "test",
                        Name = "win"
                    },
                    NewId = 2007,
                    Nets = nets1007Vm
                },
                new()
                {
                    Template = new Template()
                    {
                        WithVmbr0 = true, // по сути не нужно, ибо мы может на прямую смотреть если ли vmbr0
                        Id = 1008,
                        Password = "pass",
                        Name = "name"
                    },
                    NewId = 2008,
                    Nets = router
                }
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