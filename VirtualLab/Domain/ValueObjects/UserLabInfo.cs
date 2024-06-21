using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.ValueObjects;

public class UserLabInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    public Guid Status { get; set; }
    public int Rate { get; set; }


    public static UserLabInfo From(Lab guid, UserLab userLab)
    {
        return new UserLabInfo
        {
            Id = guid.Id,
            Goal = guid.Description,
            Manual = guid.Manual,
            Rate = userLab.Rate,
            Name = guid.Name,
            Status = userLab.StatusId
        };
    }
}