using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ProxmoxResultExtensions
{
    public static Result Match(this Corsinvest.ProxmoxVE.Api.Result result,
        Func<Result> success,
        Func<string, string> notSuccess)
    {
        var answer = new Result();
        // наверное это отвечает за запросы с ошибкой на уровне api
        if (!result.IsSuccessStatusCode)
        {
            answer = Result.Fail(notSuccess(result.ReasonPhrase));
        }

        // наверное это отвечает за ошибки уже внутри proxmox при попытки уже что-то сделать.
        if (result.ResponseInError)
        {
            answer.WithError(result.GetError());
            return answer;
        }
        
        return success();
    }
}