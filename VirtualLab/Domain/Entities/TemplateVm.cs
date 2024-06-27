
namespace VirtualLab.Domain.Entities;

public class TemplateVm : IEntity<Guid>
{
    public Guid Id { get; set; }
    public int PveTemplateId { get; set; }
    public string userName { get; set; }
    public string Password { get; set; }
}