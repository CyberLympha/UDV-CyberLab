using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Entities;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface ILabRepository : IRepositoryBase<Guid, System.Guid>
{
    
}