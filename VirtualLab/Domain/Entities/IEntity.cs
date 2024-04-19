namespace ProxmoxApi.Domen;

public interface IEntity<T>
{
    public T Id { get; set; }
}