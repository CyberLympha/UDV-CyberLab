namespace VirtualLab.Domain.Entities;

public class StatusesLab : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}