namespace VirtualLab.Domain.Value_Objects.Proxmox;

public class Template
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool WithVmbr0 { get; set; }
}