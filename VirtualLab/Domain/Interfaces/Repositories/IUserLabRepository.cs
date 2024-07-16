using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IUserLabRepository : IRepositoryBase<UserLab, Guid>
{
    public Task<Result<UserLab[]>> GetAllByUserId(Guid userId);
    public Task<Result<UserLab>> Get(Guid userId, Guid labId);
}