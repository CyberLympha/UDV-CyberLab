using System.Collections.Immutable;
using System.Data;
using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;

namespace VirtualLab.Application;

public class VirtualMachineDataHandler : IVirtualMachineDataHandler
{
    private readonly IVirtualMachineRepository _vms;
    private readonly ICredentialRepository _credentials;

    public VirtualMachineDataHandler(IVirtualMachineRepository vms,
        ICredentialRepository credentials)
    {
        _vms = vms;
        _credentials = credentials;
    }

    public async Task<Result> AddVm(VirtualMachine virtualMachine)
    {
        //todo: проверки везде

        var result = await _vms.Insert(virtualMachine);

        //
        if (result.IsFailed)
            return Result.Fail(ApiResultError
                .WithDataBase
                .NotInsert(nameof(virtualMachine), virtualMachine.Id.ToString(), result.ToApiResponse()));

        return Result.Ok();
    }

    public async Task<Result> AddCredential(Credential credential)
    {
        var result = await _credentials.Insert(credential);
        if (result.IsFailed)
            return Result.Fail(ApiResultError
                .WithDataBase
                .NotInsert(nameof(credential), credential.Id.ToString(), result.ToApiResponse()));

        return Result.Ok();
    }

    public async Task<Result<ImmutableArray<VirtualMachine>>> GetAllByUserLabId(Guid userLabId)
    {
        // todo: если где-то как здесь пусто. значит нету никаких проверок и нужно их добавить. например, а есть ли ваще такая лаба?
        
        var getVms = await _vms.GetAllByUserLab(userLabId);
        if (!getVms.TryGetValue(out var vms)) return getVms;


        return vms;
    }


    public async Task<Result> DeleteAllByUserLabId(Guid userLabId)
    {
                // todo: если где-то как здесь пусто. значит нету никаких проверок и нужно их добавить. например, а есть ли ваще такая лаба?

        return await _vms.DeleteByUserLabId(userLabId);
    }

}