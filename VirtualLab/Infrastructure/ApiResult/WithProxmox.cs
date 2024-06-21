namespace VirtualLab.Infrastructure.ApiResult;

public class WithProxmox
{
    public NetworkApiResult Network { get; } = new();
    public VmApiResult Vm { get; } = new();

    public string ChangeInterfaceFailure(string error, string node, int qemu)
    {
        return $"On node {node} not changed interfaces qemu {qemu} because: {error}";
    }

    public string CreateCloneFailure(string error)
    {
        return $"Clone Not Make Because: {error}";
    }

    // это ошибки связанные с Network
    public string NetworkDeleteFailure(string error, string node)
    {
        return $"Node : {node} not delete network because: {error}";
    }

    public string VmStopFailure(string error, string node, int qemu)
    {
        return $"vm {qemu} on Node: {node} not stopped because: {error}";
    }

    public string VmGetStatusFailure(string error, string node, int qumu)
    {
        return $"vm {qumu} on node: {node} can't get status because: {error}";
    }
}