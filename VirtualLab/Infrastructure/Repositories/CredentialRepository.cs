using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Infrastructure.DataBase;

namespace VirtualLab.Infrastructure.Repositories;

public class CredentialRepository : RepositoryBase<Credential, Guid>, ICredentialRepository
{
    public CredentialRepository(LabDbContext dbContext) : base(dbContext)
    {
    }

    public string GetAllByUserLab(Guid userLabId)
    {
        throw new NotImplementedException();
    }
}