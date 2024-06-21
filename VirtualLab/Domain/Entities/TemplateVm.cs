using VirtualLab.Controllers.TemplateController.Dto;

namespace VirtualLab.Domain.Entities;

public class TemplateVm : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public Guid LabId { get; set; }
    public int PveTemplateId { get; set; }
    public string userName { get; set; }
    public string Password { get; set; }

    public static TemplateVm From(TemplateAddRequest request)
    {
        return new TemplateVm()
        {
            Id = Guid.NewGuid(),
            Password = request.Paasword,
            userName = request.userName,
            LabId = request.LabId,
            PveTemplateId = request.templatePveId
        };
    }
}