namespace VirtualLab.Domain.Entities;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
}