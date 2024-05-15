using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure.ApiResult;

public class WithProxmox
{
    public NetworkApiResult Network { get; } = new NetworkApiResult();
    
    public string VmStartFailure(string error, LaunchVm request) => $"vm {request} not start Because {error}";
    public string ChangeIntefaceFailure(string error, UpdateInterfaceForVm updateInterfaceForVm) => $"vm {updateInterfaceForVm}, not changed because: {error}";
    public  string CreateCloneFailure(string error) => $"Clone Not Make Because: {error}";
    // это ошибки связанные с Network
    public string NetworkCreateError(string error, string node) => $"Node: {node} not created network because: {error}";

    public string NetworkDeleteFailure(string error, string node) =>
        $"Node : {node} not delete network because: {error}";

    public string VmStopFailure(string error, string node, int qemu) =>
        $"vm {qemu} on Node: {node} not stopped because: {error}";

    public string VmGetStatusFailure(string error, string node, int qumu) =>
        $"vm {qumu} on node: {node} can't get status because: {error}";
}