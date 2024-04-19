using ProxmoxApi.Domen.Entities;

namespace VirtualLab.Domain.Value_Objects;

public class UserLabsInfo
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    public Guid Status { get; set; }
    public int Rate { get; set; }


    public static UserLabsInfo From(Lab lab, UserLab userLab)
    {
        return new UserLabsInfo
        {
            Id = lab.Id,
            Goal = lab.Goal,
            Manual = lab.Manual,
            Rate = userLab.Rate,
            Name = lab.Name,
            Status = userLab.StatusId
        };
    }
}