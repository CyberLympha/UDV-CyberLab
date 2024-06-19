namespace VirtualLab.Domain.Entities.Mongo;

public class TemplateVmConfig
{
    public bool WithVmbr0 { get; set; } 
    public int TemplateId { get; set; }
    public List<int> Nets { get; set; } 
}