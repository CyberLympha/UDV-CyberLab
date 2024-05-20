using VirtualLab.Domain.Entities.Enums;

namespace VirtualLab.Domain.Entities;

public class StatusUserLab : IEntity<Guid>
{
    public Guid Id { get; set; }
    public StatusUserLabEnum Name { get; set; }
}