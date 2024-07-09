using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.ProxmoxStructure;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class TemplateData : IHaveNets
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    
    public NetCollection Nets { get; set; }
    public string Node { get; set; }


    public static TemplateData From(TemplateConfig templateDataRequest)
    {
        var nets = new NetCollection();
        foreach (var net in templateDataRequest.Nets)
        {
            nets.Add(Net.From(net));
        }

        var templateData = new TemplateData()
        {
            Id = templateDataRequest.Id,
            Name = templateDataRequest.Username,
            Node = templateDataRequest.Node,
            Password = templateDataRequest.Password,
            Nets = nets
        };

        return templateData;
    }
}