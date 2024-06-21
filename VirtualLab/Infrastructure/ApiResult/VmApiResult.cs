namespace VirtualLab.Infrastructure.ApiResult;

public class VmApiResult
{
    public string Delete(string node, int qemu, string errors)
    {
        return $"On node {node} not delete qemu {qemu} because: {errors}";
    }

    public string Start(string error, string node, int qemu)
    {
        return $"On node {node} not start qemu {qemu} Because: {error}";
    }
}