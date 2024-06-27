using VirtualLab.Domain.Interfaces.Proxmox;
namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class TemplateData : IHaveNets
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public NetCollection Nets { get; set; }
    public string Node { get; set; }
}