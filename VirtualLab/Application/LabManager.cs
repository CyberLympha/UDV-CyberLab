using Corsinvest.ProxmoxVE.Api;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using Vostok.Logging.Abstractions;
using Result = FluentResults.Result;

namespace VirtualLab.Application;

public class LabManager : ILabManager
{
    private readonly IUserLabProvider _userLabProvider;
    private readonly ILabConfigure _labConfigure;
    private readonly ILabVirtualMachineManager _labVirtualMachineManager;
    private readonly ILabEntryPointService _labEntryPointService;
    private readonly ILog _log;

    public LabManager(IUserLabProvider userLabProvider, ILabConfigure labConfigure,
        ILabVirtualMachineManager labVirtualMachineManager, ILabEntryPointService labEntryPointService, ILog log)
    {
        _userLabProvider = userLabProvider;
        _labConfigure = labConfigure;
        _labVirtualMachineManager = labVirtualMachineManager;
        _labEntryPointService = labEntryPointService;
        _log = log;
    }

    public async Task<Result<IReadOnlyList<LabEntryPoint>>> StartNew(Guid labId)
    {
        var getUserLab = await _userLabProvider.GetInfo(System.Guid.NewGuid(), labId);
        if (getUserLab.IsFailed) return Result.Fail(getUserLab.Errors);
        
        // if (getUserLab.Value.Status == ) проверка, что лаба еще не запущена.
        
        var getTemplateConfig = await _labConfigure.GetTemplateConfig(labId);
        if (getTemplateConfig.IsFailed) return Result.Fail(getTemplateConfig.Errors);

        var getConfigCurLab = await _labConfigure.GenerateLabConfig(getTemplateConfig.Value);
        if (getConfigCurLab.IsFailed) return Result.Fail(getConfigCurLab.Errors);
        
        var labCreateWithEntryPoints = await _labVirtualMachineManager.CreateLab(getTemplateConfig.Value); // пока мы возвращаем только один EntryPoint.
        if (labCreateWithEntryPoints.IsFailed) return Result.Fail(labCreateWithEntryPoints.Errors);

        foreach (var labEntryPoint in labCreateWithEntryPoints.Value)
        {
            labEntryPoint.UserLabId = getUserLab.Value.Id;
        }
        
        var save = await _labEntryPointService.InsertAll(labCreateWithEntryPoints.Value);
        if (save.IsFailed) return Result.Fail(save.Errors);

        return Result.Ok(labCreateWithEntryPoints.Value);
    }

    public Task<Result<string>> GetStatus(Guid labId)
    {
        throw new NotImplementedException();
    }
}