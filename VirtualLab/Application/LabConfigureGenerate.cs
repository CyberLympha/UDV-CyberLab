using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
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
    private readonly ITemplateVmRepository _templatesVms;
    private readonly IUnitOfWork _unitOfWorkMongo;
    private readonly IVirtualMachineDataHandler _virtualMachines;

    public LabConfigure(ILabRepository labs,
        ILog log,
        IVirtualMachineDataHandler virtualMachines,
        IProxmoxNetwork proxmoxNetwork,
        IUnitOfWork unitOfWorkMongo,
        IProxmoxResourceManager PveResourceManager,
        ITemplateVmRepository templatesVms)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
        _virtualMachines = virtualMachines;
        _proxmoxNetwork = proxmoxNetwork;
        _unitOfWorkMongo = unitOfWorkMongo;
        _pveResourceManager = PveResourceManager;
        _templatesVms = templatesVms;
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
        // будем получать доступные вм (будет доп сервис с этим) и если это вм занята, то мы просто повторим поиск доступных вм
        standCreateConfig.Node = standConfig.Node;
        var getFreeQemuIds = await _pveResourceManager
            .GetFreeQemuIds(standConfig.Node, standConfig.TemplatesVmConfig.Count);
        if (!getFreeQemuIds.TryGetValue(out var freeQemuIds, out var errors)) Result.Fail(errors);
        

        var getTemplatesVms = await GetTemplatesVms(standConfig.TemplatesVmConfig);
        if (!getTemplatesVms.TryGetValue(out var templateVms, out errors)) return Result.Fail(errors);

        for (var i = 0; i < standConfig.TemplatesVmConfig.Count; i++)
        {
            // todo: здесь как то всё слишком разношерстно! хочется одну сущность, хотя тогда это уже будет StandConfig
            var template = new TemplateData() // возможен баг при условий больго кол во пользователей.
            {
                WithVmbr0 = standConfig.TemplatesVmConfig[i].WithVmbr0,
                Id = standConfig.TemplatesVmConfig[i].TemplateId,
                Name = templateVms[i].userName,
                Password = templateVms[i].Password
            };
            standCreateConfig.CloneVmConfig.Add(
                new CloneVmConfig
                {
                    NewId = freeQemuIds[i],
                    TemplateData = template,
                    Nets = new NetCollection()
                }); //todo: Nets мы пока тоже не создаём.
        }

        return standCreateConfig;
    }

    private async Task<Result<IReadOnlyList<TemplateVm>>> GetTemplatesVms(List<TemplateVmConfig> TemplatesVmConfig)
    {
        var templatesVms = new List<TemplateVm>();
        foreach (var vmConfig in TemplatesVmConfig)
        {
            var getTemplate = await _templatesVms.GetByTemplatePveVmId(vmConfig.TemplateId);
            if (getTemplate.TryGetValue(out var template)) return Result.Fail(getTemplate.Errors);

            templatesVms.Add(template);
        }

        return templatesVms;
    }
}