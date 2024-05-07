using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.ApiResult;
using VirtualLab.Infrastructure.Extensions;

namespace VirtualLab.Application;

public class VirtualMachineService : IVirtualMachineService
{
    private readonly IVirtualMachineRepository _vms;
    private readonly ICredentialRepository _credentials;

    public VirtualMachineService(IVirtualMachineRepository vms, ICredentialRepository credentials)
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

    public Task<Result> GetAllByLabId(UserLab lab)
    {
        throw new NotImplementedException();
    }
}