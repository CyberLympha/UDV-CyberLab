using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure.ApiResult;

public class WithProxmox
{
    public NetworkApiResult Network { get; } = new NetworkApiResult();
    public VmApiResult Vm { get; } = new VmApiResult();
  
    public string ChangeInterfaceFailure(string error, string node, int qemu) => $"On node {node} not changed interfaces qemu {qemu} because: {error}";
    public  string CreateCloneFailure(string error) => $"Clone Not Make Because: {error}";
    // это ошибки связанные с Network
    public string NetworkDeleteFailure(string error, string node) =>
        $"Node : {node} not delete network because: {error}";

    public string VmStopFailure(string error, string node, int qemu) =>
        $"vm {qemu} on Node: {node} not stopped because: {error}";

    public string VmGetStatusFailure(string error, string node, int qumu) =>
        $"vm {qumu} on node: {node} can't get status because: {error}";
}