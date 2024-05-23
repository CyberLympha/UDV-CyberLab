using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static string ToApiResponse(this IResultBase result)
    {
        return string.Join($"{Environment.NewLine}", result.Errors); //todo: лучше поменять реализацию
    }

    public static bool TryGetValue<Tvalue>(this IResult<Tvalue> result, out Tvalue? value, out List<IError>? error)
    {
        error = result.Errors;
        value = result.Value;
        return result.IsSuccess;
    }

    public static bool TryGetValue<Tvalue>(this IResult<Tvalue> result, out Tvalue value)
    {
        value = result.Value;
        return result.IsSuccess;
    }

    public static bool  IsFailedWithErrors(this Result result, out List<IError> errors)
    {
        errors = result.Errors;
        return result.IsFailed;
    }
}