using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.ValueObjects;

public class UserLabInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Manual { get; set; }
    public Guid Status { get; set; }
    public int Rate { get; set; }


    public static UserLabInfo From(Lab lab, UserLab userLab)
    {
        return new UserLabInfo
        {
            Id = lab.Id,
            Description = lab.Description,
            Manual = lab.Manual,
            Rate = userLab.Rate,
            Name = lab.Name,
            Status = userLab.StatusId
        };
    }
}