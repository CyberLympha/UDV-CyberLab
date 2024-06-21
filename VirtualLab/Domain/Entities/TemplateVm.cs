namespace VirtualLab.Domain.Entities;

public class TemplateVm : IEntity<Guid>
{
    public Guid LabId { get; set; }
    public int PveTemplateId { get; set; }
    public string userName { get; set; }
    public string Password { get; set; }
    public Guid Id { get; set; }
}