using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Application;

public class LabManager : ILabManager
{
    private readonly IUserLabProvider _userLabProvider;
    private readonly ILabConfigure _labConfigure;
    private readonly ILabVirtualMachineManager _labVirtualMachineManager;
    private readonly IVirtualMachineService _virtualMachineService;
    private readonly ILog _log;

    public LabManager(IUserLabProvider userLabProvider, ILabConfigure labConfigure,
        ILabVirtualMachineManager labVirtualMachineManager, ILog log,
        IVirtualMachineService virtualMachineService)
    {
        _userLabProvider = userLabProvider;
        _labConfigure = labConfigure;
        _labVirtualMachineManager = labVirtualMachineManager;
        _log = log;
        _virtualMachineService = virtualMachineService;
    }

    public async Task<Result<ReadOnlyCollection<Credential>>> StartNew(Guid labId) //todo: пользователя нет в аргументах)
    {
        var getUserLab = await _userLabProvider.GetUserLab(Guid.NewGuid(), labId);
        if (getUserLab.IsFailed) return Result.Fail(getUserLab.Errors);

        // if (getUserLab.Value.Status == ) проверка, что лаба еще не запущена.

        var config = await GetConfig(labId);
        if (config.IsFailed) return Result.Fail(config.Errors);

        var labCreatedWithVm = await _labVirtualMachineManager.CreateLab(config.Value);
        if (labCreatedWithVm.IsFailed) return Result.Fail(labCreatedWithVm.Errors);

        // у этого foreach как-то много ответственности)))
        var result = new List<Credential>();
        foreach (var virtualMachineInfo in labCreatedWithVm.Value)
        {
            var vm = VirtualMachine.From(virtualMachineInfo.Node, virtualMachineInfo.ProxmoxVmId, getUserLab.Value.Id);
            await _virtualMachineService.AddVm(vm);
            if (string.IsNullOrEmpty(virtualMachineInfo.Ip)) continue;

            var credential = Credential.From(
                virtualMachineInfo.Ip, 
                virtualMachineInfo.Username,
                virtualMachineInfo.Password, 
                vm.Id
                );
            await _virtualMachineService.AddCredential(credential); // по сути, можно разделить на два интерфейса ICre и I Vm.
            
            result.Add(credential);
        }

        return Result.Ok(result.AsReadOnly());
    }

    //todo: по сути можно вынести в класс ConfigGenerate.
    private async Task<Result<LabConfig>> GetConfig(Guid labId)
    {
        var getTemplateConfig = await _labConfigure.GetTemplateConfig(labId);
        if (getTemplateConfig.IsFailed) return Result.Fail(getTemplateConfig.Errors);

        var getConfigCurLab = await _labConfigure.GenerateLabConfig(getTemplateConfig.Value);
        if (getConfigCurLab.IsFailed) return Result.Fail(getConfigCurLab.Errors);

        return getConfigCurLab;
    }

    public Task<Result<string>> GetStatus(Guid labId)
    {
        throw new NotImplementedException();
    }
}