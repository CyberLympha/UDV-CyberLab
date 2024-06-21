using VirtualLab.Controllers.LabDistributionController.Dto;

namespace VirtualLab.Domain.Entities;

//todo написать нормально сущность
public class Lab : IEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeadLine { get; set; }
    public Guid CreatedBy { get; set; }
    public string Manual { get; set; }
    public bool IsOpened { get; set; }

    public List<UserLab> UserLabs { get; set; }
    public Guid Id { get; set; }

    public static Lab From(LabCreateRequest request) // а что если это реализовывать на уровне контроллеера?
    {
        return new Lab
        {
            Description = request.Description,
            Id = Guid.NewGuid(),
            Manual = request.Manual,
            Name = request.Name,
            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"), //TODO: брать id из Identity
            UserLabs = new List<UserLab>()
        };
    }
}