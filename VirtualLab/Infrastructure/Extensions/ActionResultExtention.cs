using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ProxmoxApi;

public static class ActionResultExtensions
{
    public static ActionResult<T> Match<T>(
        this Result<T> result,
        Func<T,ActionResult<T>> onSuccess,
        Func<List<IReason>, ActionResult<T>> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Reasons);
    }

    public static ActionResult Match(
        this Result result,
        Func<ActionResult> onSuccess,
        Func<List<IReason>, ActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Reasons);
    }
    /*public static ActionResult<Result> IsSucceed(this Result result)
    {
        return result.IsSucceed() ? new OkResult() : Bad
    }*/
}