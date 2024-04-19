using VirtualLab.Controllers.LabCreationService.Dto;

namespace ProxmoxApi.Domen.Entities;


//todo 
public class Lab : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    
    //todo по идей, было бы здорово засунуть это в другую сущность, но пока, так как mvp будет здесь.
    public long Node { get; set; }
    public long VmIdTemplate { get; set; } // здесь будет id template либо id Vm на первых этапах.
    public string UserNameVm { get; set; }
    public string PasswordVm { get; set; }

    public static Lab From(LabCreateRequest request) // а что если это реализовывать на уровне контроллеера?
    {
        return new Lab()
        {
            Goal = request.Goal,
            Id = Guid.NewGuid(),
            Manual = request.Manual,
            Name = request.Name,
            VmIdTemplate = 1,
            PasswordVm = request.Password,
            UserNameVm = request.UserName,
            Node = request.Node
        };
    } 
}