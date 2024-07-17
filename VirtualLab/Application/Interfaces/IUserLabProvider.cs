using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Enums;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application.Interfaces;

public interface IUserLabProvider
{
    // по сути мы должны смотреть, а какие лабы уже есть, вообще и если есть новые, то добавлять их к юзеру. то есть с использованием репозитрия сущности Lab
    public Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(Guid user); // todo: по guid. а не User.
    public Task<Result<UserLabInfo>> GetUserLabInfo(Guid userId, Guid labId);
    Task<Result> UpdateStatus(Guid userLabId, StatusUserLabEnum run);
}