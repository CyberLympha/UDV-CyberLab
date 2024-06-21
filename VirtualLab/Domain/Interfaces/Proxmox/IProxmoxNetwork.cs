using FluentResults;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Interfaces.Proxmox;

public interface IProxmoxNetwork
{
    // потенциально 2
    public Task<Result<NetCollection>>
        GetAllNetworksBridgeByVm(int vmId, string node); // GET /api2/json/nodes/{node}/network

    // либо 2 либо 3
    public Task<Result> Create(string node, Net net); // мы здесь создаём новый интерфейс

    public Task<Result> Remove(string node, Net net);

    // 3 либо 4 
    public Task<Result> Apply(string node); // PUT /api2/json/nodes/{node}/network применяем обновления
}