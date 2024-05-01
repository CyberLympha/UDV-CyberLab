using ProxmoxApi.Domen;
using VirtualLab.Controllers.LabCreationService.Dto;

namespace VirtualLab.Domain.Entities;


//todo 
public class Guid : IEntity<System.Guid>
{
    public System.Guid Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    

    public static Guid From(LabCreateRequest request) // а что если это реализовывать на уровне контроллеера?
    {
        return new Guid()
        {
            Goal = request.Goal,
            Id = System.Guid.NewGuid(),
            Manual = request.Manual,
            Name = request.Name,
        };
    } 
}