using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Enums;

namespace VirtualLab.Domain.Interfaces.Repositories;

public interface IStatusUserLabRepository : IRepositoryBase<StatusUserLab, Guid>
{
    public Task<Result<StatusUserLab>> Get(StatusUserLabEnum statusUserLabEnum);
}