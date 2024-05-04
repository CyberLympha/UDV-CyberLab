using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects;

namespace VirtualLab.Application;

public class UserLabProviderService : IUserLabProvider
{
    private readonly ILabRepository _labs;
    private readonly IUserLabRepository _userLabs;

    public UserLabProviderService(ILabRepository labs, IUserLabRepository userLabs)
    {
        _labs = labs;
        _userLabs = userLabs;
    }

    //todo: сделать норм реализацию. здесь как минимум можно сделать один sql запрос, который будет решать половину логики. сейчас это кринж, и очень медленно. слишком медленно
    public async Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(User user)
    {
        // кароче, это кринж.
        var userLabsResult = await _userLabs.GetAllByUserId(user.Id);
        if (userLabsResult.IsFailed)
        {
            return Result.Fail(userLabsResult.Errors);
        }

        var labsResult = await _labs.GetAll();
        if (labsResult.IsFailed)
        {
            return Result.Fail(labsResult.Errors);
        }

        if (labsResult.Value.Length != userLabsResult.Value.Length)
        {
            foreach (var lab in labsResult.Value)
            {
                var isExist = false;
                foreach (var userLab in userLabsResult.Value)
                {
                    if (lab.Id == userLab.LabId)
                    {
                        isExist = true;
                    }
                }

                if (isExist) continue;

                var userLabs = UserLab.From(user, lab);
                var result = await _userLabs.Insert(userLabs);

                if (result.IsFailed)
                {
                    return Result.Fail(result.Errors);
                }
            }

            userLabsResult = await _userLabs.GetAllByUserId(user.Id);
        }

        var answer = GetUseLabsInfos(labsResult.Value, userLabsResult.Value);

        return answer;
    }

    public Task<Result<UserLabInfo>> GetInfo(System.Guid userId, System.Guid labId)
    {
        throw new NotImplementedException();
    }
    

    private static List<UserLabInfo> GetUseLabsInfos(IEnumerable<Lab> labs, UserLab[] userLabs)
    {
        var answer = new List<UserLabInfo>();
        foreach (var lab in labs)
        {
            foreach (var useLab in userLabs)
            {
                if (lab.Id == useLab.LabId)
                {
                    answer.Add(UserLabInfo.From(lab, useLab));
                }
            }
        }

        return answer;
    }
}