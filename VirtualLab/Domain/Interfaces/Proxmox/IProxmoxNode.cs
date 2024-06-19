using FluentResults;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IProxmoxNode
{
    public Task<Result<List<int>>> GetAllQemu(string node);
}