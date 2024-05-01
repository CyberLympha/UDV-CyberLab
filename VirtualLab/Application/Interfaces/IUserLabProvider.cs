using FluentResults;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Application.Interfaces;

public interface IUserLabProvider
{
    // по сути мы должны смотреть, а какие лабы уже есть, вообще и если есть новые, то добавлять их к юзеру. то есть с использованием репозитрия сущности Lab
    public Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(User user); // todo: по guid. а не User.
    public Task<Result<UserLabInfo>> GetInfo(Guid userId, Guid labId);
    
}