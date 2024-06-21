namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class TemplateData
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool WithVmbr0 { get; set; }
}