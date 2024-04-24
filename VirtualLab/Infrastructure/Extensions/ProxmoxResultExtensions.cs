using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxResultExtensions
{
    public static Result Match(this Corsinvest.ProxmoxVE.Api.Result result,
        Func<Result> success,
        Func<string, string> notSuccess,
        Func<string, string> responseError)
    {
        // наверное это отвечает за запросы с ошибкой на уровне api
        if (!result.IsSuccessStatusCode)
        {
            return Result.Fail(notSuccess(result.ReasonPhrase));
        }

        // наверное это отвечает за ошибки уже внутри proxmox при попытки уже что-то сделать.
        if (result.ResponseInError)
        {
            return Result.Fail(responseError(result.GetError()));
        }

        return success();
    } 
}