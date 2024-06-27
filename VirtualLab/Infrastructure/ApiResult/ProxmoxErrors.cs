namespace VirtualLab.Infrastructure.ApiResult;

public class ProxmoxErrors
{
    public ApiNetworkErrors ApiNetworkErrors { get; } = new();
    public VmApiResult Vm { get; } = new();
    public NodeApiResult Node { get; } = new();
    public string ChangeInterfaceFailure(string errors, string node, int qemu)
    {
        return $"On node {node} not changed interfaces qemu {qemu} because: {errors}";
    }

    

  

  
}