namespace VirtualLab.Domain.Value_Objects.Proxmox;

// todo: сделать лучше
public class Net
{
    private int _number;
    private string _value;
    public KeyValuePair<int, string> Get => new(_number, _value);

    // по сути можно заюзать паттерн builder, в потенциале, если запросы будут более сложные.
    public Net(string model, string bridge, int number)
    {
        _number = number;
        _value = $"model={model},bridge={bridge}";
    }
}