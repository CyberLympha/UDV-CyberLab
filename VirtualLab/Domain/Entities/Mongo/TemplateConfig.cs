namespace VirtualLab.Domain.Entities.Mongo;

public class TemplateConfig
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Node { get; set; }
    
    public List<NetConfig> Nets { get; set; }

}