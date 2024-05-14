using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure.ApiResult;

public class WithProxmox
{
    public string VmStartFailure(string error, LaunchVm request) => $"vm {request} not start Because {error}";
    public string ChangeIntefaceFailure(string error, UpdateInterfaceForVm updateInterfaceForVm) => $"vm {updateInterfaceForVm}, not changed because: {error}";
    public  string CreateCloneFailure(string error) => $"Clone Not Make Because: {error}";
    // это ошибки связанные с Network
    public string NetworkApplyError(string error, string node) => $"Node: {node} not applied because: {error}";
    public string NetworkCreateError(string error, string node) => $"Node: {node} not created network because: {error}";

    public string NetworkDeleteFailure(string error, string node) =>
        $"Node : {node} not delete network because: {error}";

    public string VmStopFailure(string error, string node, int qemu) =>
        $"vm {qemu} on Node: {node} not stopped because: {error}";
}