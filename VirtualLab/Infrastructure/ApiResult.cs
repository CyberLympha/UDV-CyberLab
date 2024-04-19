using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Infrastructure;

public class ApiResultError
{
    public static string NotFound(string nameEntity) => $"Not Found {nameEntity}";
    
    
    
    // это ошибки связанные с вм
    public static string GenerateTemplateCloneFailure(string error) => $"Clone Not Make Because: {error}";
    public static string VmStartFailure(string error, LaunchVm request) => $"vm {request} not start Because {error}";

    public static string ChangeIntefaceFailure(string error, ChangeInterfaceForVm changeInterfaceForVm) =>
        $"vm {changeInterfaceForVm}, not changed because: {error}";
    
    // это ошибки связанные с Network
    public static string NetworkApplyError(string error, string node) => $"Node: {node} not applied because: {error}";

    public static string NetworkCreateError(string error, string node) =>
        $"Node: {node} not created network because: {error}";
}


//todo сделать классы с ответами под разные категорий.