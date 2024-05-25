using System;
//using VirtualLab.Domain.Entities.Enums;

namespace VirtualLab.Domain.Entities;

public class StatusUserLab : IEntity<Guid>
{
    public Guid Id { get; set; }
    //public Enums.StatusUserLabEnum Name { get; set; } вернуть, сейчас не работает
    public string Name { get; set; }
}