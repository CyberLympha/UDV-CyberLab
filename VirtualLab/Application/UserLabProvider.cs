using FluentResults;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.ValueObjects;

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

    //Todo: сделать норм реализацию. здесь как минимум можно сделать один sql запрос, который будет решать половину логики. сейчас это кринж, и очень медленно. слишком медленно
    public async Task<Result<IReadOnlyCollection<UserLabInfo>>> GetInfoAll(User user)
    {
        // кароче, это кринж.
        // todo: нету проверки на пользователя, и пока не будет, ибо чтоб его найти нужно лазить в бд, которая потенциально может находиться где угодно. на другом сервере, как пример.
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

    public async Task<Result<UserLabInfo>> GetUserLab(Guid userId, Guid labId)
    {
        return Result.Ok(new UserLabInfo()
        {
            Goal = "Afd",
            Id = labId,
            Name = "dfas",
            Manual = "Dfasdf",
            Rate = 234,
            Status = Guid.NewGuid(),
        });
        
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

    public async Task<Result<IReadOnlyCollection<AttemptShortInfo>>> GetAllCompletedByLabId(Guid labId)
    {
        var userLabsResult = await _userLabs.GetAllCompletedByLabId(labId);
        if (userLabsResult.IsFailed)
            return Result.Fail(userLabsResult.Errors);
        var answer = new List<AttemptShortInfo>();
        foreach (var userLab in userLabsResult.Value)
        {
            var userInfo = await GetUserInfo(userLab.UserId.ToString());
            answer.Add(AttemptShortInfo.From(userInfo, userLab));
        }
        return answer;
    }

    private async Task<UserInfo> GetUserInfo(string userId)
    {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri("https://localhost:7182");
            response = await httpClient.GetAsync($"api/auth/users/{userId}");
        }
        response.EnsureSuccessStatusCode();
        var contentStream = await response.Content.ReadAsStreamAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var userResponse = await JsonSerializer.DeserializeAsync<UserInfo>(contentStream, options);
        return userResponse;
    }

    public async Task<Result<IReadOnlyCollection<TeacherLabShortInfo>>> GetTeacherLabs(Guid teacherId)
    {
        var labsResult = await _labs.GetAllByCreatorId(teacherId);
        if (labsResult.IsFailed)
            return Result.Fail(labsResult.Errors);
        var answer = new List<TeacherLabShortInfo>();
        foreach (var lab in labsResult.Value)
        {
            answer.Add(TeacherLabShortInfo.From(lab));
        }
        return answer;
    }
}