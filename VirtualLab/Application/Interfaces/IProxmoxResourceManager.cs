using FluentResults;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Application.Interfaces;

public interface IProxmoxResourceManager // как идея можно сделать абстрактую фабрику под каждую node и тогда здесь из аргументов можно будет убрать node. а сама node будет прописана внутри этого класса.
{
    public Task<Result<QemuCollections>> GetFreeQemuIds(string node, long count);
    public Task<Result<List<long>>> GetFreeVmbrs(string node, int count);
}