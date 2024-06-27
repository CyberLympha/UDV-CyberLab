using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxResultExtensions
{
    public static Result Match(this Corsinvest.ProxmoxVE.Api.Result result,
        Func<Result> success,
        Func<string, string> errors)
    {
        return !result.IsSuccessStatusCode
            ? Result.Fail(errors(result.ReasonPhrase)).WithError(result.GetError())
            : success();
    }

    public static Result Fail(this Corsinvest.ProxmoxVE.Api.Result result,
        Func<string, string> errors)
    {
        return Result.Fail(errors(result.ReasonPhrase)).WithError(result.GetError());
    } 
}