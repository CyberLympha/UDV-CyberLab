namespace VirtualLab.Domain.Entities.Mongo;

public class TemplateVmConfig
{
    // здесь должы хранится только TemplateId и Nets и всё мы по TemplateId уже достанем сам template. то есть в confgi будет только templateId и Nets.
    public bool WithVmbr0 { get; set; }
    public int TemplateId { get; set; }
    public List<int> Nets { get; set; }
}