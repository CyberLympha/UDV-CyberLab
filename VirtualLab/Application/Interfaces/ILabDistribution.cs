using FluentResults;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Application;

public interface ILabProvider
{
    public Task<Result<IReadOnlyCollection<UserLabsInfo>>> GetAllByUser(User user); // по сути мы должны смотреть, а какие лабы уже есть, вообще и если есть новые, то добавлять их к юзеру. то есть с использованием репозитрия сущности Lab
    
}




