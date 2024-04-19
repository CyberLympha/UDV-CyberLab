using FluentResults;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Application.Interfaces;

public interface INetworkService
{
    // потенциально 2
    public Task<Result> GetAllNetworks(string node); // GET /api2/json/nodes/{node}/network
    // либо 2 либо 3
    public Task<Result> Create(CreateInterface request); // мы здесь создаём новый интерфейс
    // 3 либо 4 
    public Task<Result> Apply(string node); // PUT /api2/json/nodes/{node}/network применяем обновления
   
}