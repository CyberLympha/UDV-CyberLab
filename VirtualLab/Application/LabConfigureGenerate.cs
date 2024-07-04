using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Infrastructure.Extensions;
using VirtualLab.Lib;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabConfigure : ILabConfigure
{
    private readonly ILabRepository _labs;
    private readonly ILog _log;
    private readonly IProxmoxNetwork _proxmoxNetwork;
    private readonly IProxmoxResourceManager _pveResourceManager;
    private readonly IUnitOfWork _unitOfWorkMongo;
    private readonly IVirtualMachineDataHandler _virtualMachines;

    public LabConfigure(ILabRepository labs,
        ILog log,
        IVirtualMachineDataHandler virtualMachines,
        IProxmoxNetwork proxmoxNetwork,
        IUnitOfWork unitOfWorkMongo,
        IProxmoxResourceManager PveResourceManager)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
        _virtualMachines = virtualMachines;
        _proxmoxNetwork = proxmoxNetwork;
        _unitOfWorkMongo = unitOfWorkMongo;
        _pveResourceManager = PveResourceManager;
    }


    public async Task<Result<StandCreateConfig>> GetConfigByLab(Guid labId)
    {
        var getTemplateConfig = await _unitOfWorkMongo.configs.GetByLabId(labId);
        if (getTemplateConfig.IsFailed) return Result.Fail(getTemplateConfig.Errors);

        var getConfigCurLab = await GenerateLabConfig(getTemplateConfig.Value);
        if (getConfigCurLab.IsFailed) return Result.Fail(getConfigCurLab.Errors);

        return getConfigCurLab;
    }

    public async Task<Result<StandRemoveConfig>> GetConfigByUserLab(Guid userLabId)
    {
        var resultVms = await _virtualMachines.GetAllByUserLabId(userLabId);
        if (!resultVms.TryGetValue(out var virtualMachines)) return Result.Fail($"ошибка: {resultVms.Errors}");

        var userLabConfig = new StandRemoveConfig();
        foreach (var virtualMachine in virtualMachines)
        {
            var resultNets =
                await _proxmoxNetwork.GetAllNetworksBridgeByVm(virtualMachine.ProxmoxVmId, virtualMachine.Node);
            if (!resultNets.TryGetValue(out var nets)) return Result.Fail($"error: {resultNets.Errors}");

            userLabConfig.VmsInfos.Add(new VmInfo
            {
                ProxmoxVmId = virtualMachine.ProxmoxVmId,
                Nets = nets,
                Node = virtualMachine.Node
            });
        }

        return userLabConfig;
    }

    private async Task<Result<StandCreateConfig>> GenerateLabConfig(StandConfig standConfig)
    {
        var standCreateConfig = new StandCreateConfig();

        var getFreeQemuIds = await _pveResourceManager
            .GetFreeQemuIds(standConfig.Node, standConfig.Template.Count);
        if (!getFreeQemuIds.TryGetValue(out var freeQemuIds, out var errors)) Result.Fail(errors);


        //todo: как-то не однородно
        for (var i = 0; i < standConfig.Template.Count; i++)
        {
            // да сейчас там же где и template создаётся создаётся и конфиг
            var qemu = new NewQemu()
            {
                Node = standConfig.Template[i].Node,
                Id = freeQemuIds[i]
            };
            standCreateConfig.Add(
                new CloneVmConfig
                {
                    newQemu = qemu,
                    TemplateData = TemplateData.From(standConfig.Template[i]),
                });
        }

        return standCreateConfig;
    }

    private async Task<Result<List<NewQemu>>> GetQemus(StandConfig config)
    {
        var getFreeQemuIds = await _pveResourceManager
            .GetFreeQemuIds(config.Node, config.Template.Count);
        if (!getFreeQemuIds.TryGetValue(out var freeQemuIds, out var errors)) Result.Fail(errors);

        var qemus = new List<NewQemu>();
        for (int i = 0; i < freeQemuIds.Count; i++)
        {
            qemus.Add(new NewQemu() { Node = config.Node, Id = freeQemuIds[i] });
        }

        return qemus;
    }
}