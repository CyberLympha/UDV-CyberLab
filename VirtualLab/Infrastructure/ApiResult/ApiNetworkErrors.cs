namespace VirtualLab.Infrastructure.ApiResult;

public class ApiNetworkErrors
{
    public string Apply(string error, string node)
    {
        return $"Node: {node} not applied because: {error}";
    }
    
    public string NetworkDelete(string error, string node)
    {
        return $"Node : {node} not delete network because: {error}";
    }

    public string Create(string error, string node, string iFace)
    {
        return $"not created network {iFace} on node: {node} because: {error}";
    }
}