namespace VirtualLab.Domain.Entities;

public interface IEntity<T>
{
    public T Id { get; set; }
}