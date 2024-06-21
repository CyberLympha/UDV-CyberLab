using VirtualLab.Domain.Entities.Enums;

namespace VirtualLab.Domain.Entities;

public class StatusUserLab : IEntity<Guid>
{
    public StatusUserLabEnum Name { get; set; }
    public Guid Id { get; set; }
}