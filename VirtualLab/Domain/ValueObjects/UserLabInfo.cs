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

    public static UserLabInfo From(Lab guid, UserLab userLab, StatusUserLab statusUserLab)
    {
        return new UserLabInfo
        {
            Id = guid.Id,
            Goal = guid.Description,
            Manual = guid.Manual,
            Rate = userLab.Rate,
            Name = guid.Name,
            Status = statusUserLab.Name
        };
    }
}