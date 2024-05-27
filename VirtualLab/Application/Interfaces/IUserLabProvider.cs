using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application.Interfaces;

public interface IUserLabProvider
{
    // по сути мы должны смотреть, а какие лабы уже есть, вообще и если есть новые, то добавлять их к юзеру. то есть с использованием репозитрия сущности Lab
    public Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(User user); // todo: по guid. а не User.
    public Task<Result<UserLabInfo>> GetUserLab(Guid userId, Guid labId);
    public Task<Result<IReadOnlyCollection<AttemptShortInfo>>> GetAllCompletedByLabId(Guid labId);
    public Task<Result<AttemptFullInfo>> GetAttempt(Guid userLabId);
    public Task<Result<AttemptFullInfo>> UpdateUserLabRate(Guid userLabId, int newRate);
}