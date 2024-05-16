using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface ICredentialRepository : IRepositoryBase<Credential,Guid>
{
    public string GetAllByUserLab(Guid userLabId);
}