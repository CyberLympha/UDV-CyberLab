using FluentResults;
using ProxmoxApi.Domen.Entities;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IUserLabRepository : IRepositoryBase<UserLab, Guid>
{
    public Task<Result<UserLab[]>> GetAllByUserId(Guid userId);

}