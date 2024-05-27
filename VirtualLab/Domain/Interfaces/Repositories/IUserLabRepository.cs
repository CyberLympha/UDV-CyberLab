using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IUserLabRepository : IRepositoryBase<UserLab, Guid>
{
    public Task<Result<UserLab[]>> GetAllByUserId(Guid userId);
    public Task<Result<UserLab[]>> GetAllCompletedByLabId(Guid labId);
    public Task<Result<UserLab>> UpdateRate(Guid userLabId, int newRate);
}