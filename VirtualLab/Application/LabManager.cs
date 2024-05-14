using System.Collections.ObjectModel;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Application;

public class LabManager : ILabManager
{
    private readonly IUserLabProvider _userLabProvider;
    private readonly ILabConfigure _labConfigure;
    private readonly IStandManager _standManager;
    private readonly IVirtualMachineService _virtualMachineService;
    private readonly ILog _log;

    public LabManager(IUserLabProvider userLabProvider, ILabConfigure labConfigure,
        IStandManager standManager, ILog log,
        IVirtualMachineService virtualMachineService)
    {
        _userLabProvider = userLabProvider;
        _labConfigure = labConfigure;
        _standManager = standManager;
        _log = log;
        _virtualMachineService = virtualMachineService;
    }

    public async Task<Result<ReadOnlyCollection<Credential>>> StartNew(Guid labId, Guid userId)
    {
        var getUserLab = await _userLabProvider.GetUserLab(userId, labId);
        if (getUserLab.IsFailed)
        {
            _log.Error($"Not find lab with {labId} for user with id {userId}"); // так бы везде))
            return Result.Fail(getUserLab.Errors);
        }

        // if (getUserLab.Value.Status == ) проверка, что лаба еще не запущена.

        var config = await _labConfigure.GetConfigByLab(labId);
        if (config.IsFailed) return Result.Fail(config.Errors);

        var labCreatedWithVm = await _standManager.CreateStand(config.Value);
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
            await _virtualMachineService
                .AddCredential(credential); // по сути, можно разделить на два интерфейса ICre и I Vm. а нужно ли 

            result.Add(credential);
        }

        return Result.Ok(result.AsReadOnly());
    }

    public Task<Result<string>> GetStatus(Guid labId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> End(Guid labId, Guid userId)
    {
        var getUserLab = await _userLabProvider.GetUserLab(userId, labId);
        if (getUserLab.IsFailed)
        {
            _log.Error($"Not find lab with {labId} for user with id {userId}"); // так бы везде))
            return Result.Fail(getUserLab.Errors);
        }
        // if (getUserLab.Value.Status == ) проверка, что лаба УЖЕ ЗАПУЩЕНА

        var userLabConfig = await _labConfigure.GetConfigByUserLab(getUserLab.Value.Id);
        if (!userLabConfig.TryGetValue(out var config))
        {
            return Result.Fail($"error {getUserLab.Errors}");
        }

        
        await _standManager.RemoveStand(config);
        throw new NotImplementedException();
    }
}