using FluentResults;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IProxmoxNode
{
    public Task<Result<List<int>>> GetAllQemuIds(string node);

    public Task<Result<List<int>>> GetAllIFaceId(string node);
}