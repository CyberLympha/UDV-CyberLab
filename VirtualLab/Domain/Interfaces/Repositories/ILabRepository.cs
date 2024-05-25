using FluentResults;
using VirtualLab.Domain.Entities;


namespace VirtualLab.Domain.Interfaces.Repositories;

public interface ILabRepository : IRepositoryBase<Lab, Guid>
{
    public Task<Result<Lab[]>> GetAllByCreatorId(Guid creatorId);
}