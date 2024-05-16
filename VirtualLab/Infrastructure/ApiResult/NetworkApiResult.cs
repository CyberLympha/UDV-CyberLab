namespace VirtualLab.Infrastructure.ApiResult;

public class NetworkApiResult
{
    public string Apply(string error, string node) => $"Node: {node} not applied because: {error}";
    public string Create(string error, string node, string iFace) => $"not created network {iFace} on node: {node} because: {error}";

}