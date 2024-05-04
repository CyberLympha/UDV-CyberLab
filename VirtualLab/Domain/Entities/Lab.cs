using VirtualLab.Controllers.LabCreationService.Dto;

namespace VirtualLab.Domain.Entities;


//todo 
public class Lab : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    

    public static Lab From(LabCreateRequest request) // а что если это реализовывать на уровне контроллеера?
    {
        return new Lab()
        {
            Goal = request.Goal,
            Id = Guid.NewGuid(),
            Manual = request.Manual,
            Name = request.Name,
        };
    } 
}