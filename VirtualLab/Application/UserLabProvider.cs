using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Enums;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.ValueObjects;
using VirtualLab.Infrastructure.Extensions;
using Vostok.Logging.Abstractions;
using ZstdSharp.Unsafe;

namespace VirtualLab.Application;

public class UserLabProvider : IUserLabProvider
{
    private readonly ILabRepository _labs;
    private readonly IUserLabRepository _userLabs;
    private readonly IStatusUserLabRepository _statusesUserLab;
    private readonly ILog _log;

    public UserLabProvider(ILabRepository labs, IUserLabRepository userLabs, IStatusUserLabRepository statusesUserLab,
        ILog log)
    {
        _labs = labs;
        _userLabs = userLabs;
        _statusesUserLab = statusesUserLab;
        _log = log;
    }


    //Todo: сделать норм реализацию. здесь как минимум можно сделать один sql запрос, который будет решать половину логики. сейчас это кринж, и очень медленно. слишком медленно
    public async Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(Guid userId)
    {
        // todo: нету проверки на пользователя, и пока не будет, ибо чтоб его найти нужно лазить в бд, которая потенциально может находиться где угодно. на другом сервере, как пример.
        var userLabsResult = await _userLabs.GetAllByUserId(userId);
        if (userLabsResult.IsFailed) return Result.Fail(userLabsResult.Errors);

        var labsResult = await _labs.GetAll();
        if (labsResult.IsFailed) return Result.Fail(labsResult.Errors);

        if (!(await _statusesUserLab.Get(StatusUserLabEnum.NotCreated)).TryGetValue(out var statusNotCreated,
                out var error))
        {
            return Result.Fail(error);
        }

        if (labsResult.Value.Count != userLabsResult.Value.Length)
        {
            foreach (var lab in labsResult.Value)
            {
                var isExist = false;
                foreach (var userLab in userLabsResult.Value)
                    if (lab.Id == userLab.LabId)
                        isExist = true;

                if (isExist) continue;

                var newUserLab = UserLab.From(userId, lab, statusNotCreated);
                _log.Debug($"{newUserLab}");
                var result = await _userLabs.Insert(newUserLab);

                if (result.IsFailed) return Result.Fail(result.Errors);
            }

            userLabsResult = await _userLabs.GetAllByUserId(userId);
        }

        if (!(await _statusesUserLab.GetAll()).TryGetValue(out var statusUserLab, out error))
        {
            return Result.Fail(error);
        }

        var answer = GetUseLabsInfos(labsResult.Value, userLabsResult.Value, statusUserLab);

        return answer;
    }

    public async Task<Result<UserLabInfo>> GetUserLabInfo(Guid userId, Guid labId)
    {
        if (!(await _labs.Get(labId)).TryGetValue(out var lab, out var error))
        {
            return Result.Fail(error);
        }

        if (!(await _userLabs.Get(userId, labId)).TryGetValue(out var userLab, out error))
        {
            return Result.Fail(error);
        }

        if (!(await _statusesUserLab.Get(userLab.StatusId)).TryGetValue(out var statusUserLab, out error))
        {
            return Result.Fail(error);
        }

        return Result.Ok(UserLabInfo.From(lab, userLab, statusUserLab));
    }

    public async Task<Result> UpdateStatus(Guid userLabId, StatusUserLabEnum status)
    {
        if (!(await _userLabs.Get(userLabId)).TryGetValue(out var userLab, out var error))
        {
            return Result.Fail(error);
        }

        if (!(await _statusesUserLab.Get(status)).TryGetValue(out var statusUserLab, out error))
        {
            return Result.Fail(error);
        }

        userLab.StatusId = statusUserLab.Id;
        if ((await _userLabs.Update(userLab)).IsFailedWithErrors(out var errors)) 
            return Result.Fail(errors);

        return Result.Ok();
    }


    private static List<UserLabInfo> GetUseLabsInfos(
        IEnumerable<Lab> labs,
        IEnumerable<UserLab> userLabs,
        IEnumerable<StatusUserLab> statusUserLabs)
    {
        return (
            from lab in labs
            from useLab in userLabs
            from statusUserLab in statusUserLabs
            where lab.Id == useLab.LabId && useLab.StatusId == statusUserLab.Id
            select UserLabInfo.From(lab, useLab, statusUserLab)).ToList();
    }
}