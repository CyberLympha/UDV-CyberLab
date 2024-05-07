using FluentResults;

namespace VirtualLab.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static string ToApiResponse(this IResultBase result)
    {
        return string.Join($"{Environment.NewLine}", result.Errors); //todo: лучше поменять реализацию
    }
}