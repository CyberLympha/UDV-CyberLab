namespace VirtualLab.Infrastructure.ApiResult;

public class VmApiResult
{
    public string Delete(string node, int qemu, string errors)
    {
        return $"On node {node} not delete qemu {qemu} because: {errors}";
    }

    public string VmGetStatusFailure(string error, string node, int qemu)
    {
        return $"vm {qemu} on node: {node} can't get status because: {error}";
    }

    public string VmStop(string error, string node, int qemu)
    {
        return $"vm {qemu} on Node: {node} not stopped because: {error}";
    }

    public string CreateClone(string error)
    {
        return $"Clone Not Make Because: {error}";
    }

    public string Start(string error, string node, int qemu)
    {
        return $"On node {node} not start qemu {qemu} Because: {error}";
    }
}