using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Entities;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Domain.Value_Objects;

public class UserLabInfo
{
    public System.Guid Id { get; set; }

    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    public System.Guid Status { get; set; }
    public int Rate { get; set; }


    public static UserLabInfo From(Guid guid, UserLab userLab)
    {
        return new UserLabInfo
        {
            Id = guid.Id,
            Goal = guid.Goal,
            Manual = guid.Manual,
            Rate = userLab.Rate,
            Name = guid.Name,
            Status = userLab.StatusId
        };
    }
}