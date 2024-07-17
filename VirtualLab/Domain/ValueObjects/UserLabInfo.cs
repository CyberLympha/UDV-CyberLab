using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Enums;

namespace VirtualLab.Domain.ValueObjects;

public class UserLabInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    public int Rate { get; set; }
    public StatusUserLabEnum Status { get; set; }

    public static UserLabInfo From(Lab lab, UserLab userLab, StatusUserLab statusUserLab)
    {
        return new UserLabInfo
        {
            Id = userLab.Id,
            Goal = lab.Description,
            Manual = lab.Manual,
            Rate = userLab.Rate,
            Name = lab.Name,
            Status = statusUserLab.Name
        };
    }
}