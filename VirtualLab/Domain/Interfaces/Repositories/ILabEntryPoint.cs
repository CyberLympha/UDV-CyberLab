using VirtualLab.Domain.Entities;
using Guid = System.Guid;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface ILabEntryPointRepository : IRepositoryBase<Credential, Guid>
{
    
}