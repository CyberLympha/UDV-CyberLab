using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Interfaces.Proxmox;
namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class TemplateData : IHaveNets
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public NetCollection Nets { get; set; }
    public string Node { get; set; }

    public static TemplateData From(TemplateDataRequest templateDataRequest)
    {
        var nets = new NetCollection();
        nets.AddRange(templateDataRequest.Nets);
        
        var templateData = new TemplateData()
        {
            Id = templateDataRequest.Id,
            Name = templateDataRequest.Name,
            Node = templateDataRequest.Node,
            Password = templateDataRequest.Password,
            Nets= nets
        };

        return templateData;
    } 
}