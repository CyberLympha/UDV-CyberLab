using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxResultExtensions
{
    public static Result Match(this Corsinvest.ProxmoxVE.Api.Result result,
        Func<Result> success,
        Func<string, string> notSuccess)
    {
        return !result.IsSuccessStatusCode
            ? Result.Fail(notSuccess(result.ReasonPhrase)).WithError(result.GetError())
            : success();
    }
}