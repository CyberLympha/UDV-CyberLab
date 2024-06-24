using FluentResults;

namespace VirtualLab.Infrastructure.ApiResult;

public class NodeApiResult
{
    public string GetQuemies(string node, string errors) =>
        $"can't get quemies from {node} because {errors}";
}