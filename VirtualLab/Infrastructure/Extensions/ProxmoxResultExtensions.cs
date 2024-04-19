using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxResultExtensions
{
    public static Result Match(this Corsinvest.ProxmoxVE.Api.Result result)
    {
        /*
        return result.ResponseInError ? Result.Fail(ApiResultError.VmStartFailure(result.GetError())) : Result.Ok();
        */
        throw new NotImplementedException();
    } 
}