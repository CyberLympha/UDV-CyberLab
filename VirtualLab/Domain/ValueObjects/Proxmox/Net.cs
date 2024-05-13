namespace VirtualLab.Domain.Value_Objects.Proxmox;

// todo: сделать лучше
public class Net
{
    private Dictionary<string, string> Parameters { get; set; } = new();

    
    // todo: пока что максимально простая реализация, потом допилить.
    public Net(string model, string bridge)
    {
        Parameters.Add("model", model);
        Parameters.Add("bridge", bridge);
    }

    public override string ToString()
    {
        return $"{Bridge}, {Parameters}";
    }

    public string this[string index] => Parameters[index];
    public string GetFull => string.Join(",", Parameters.Select(x => $"{x.Key}={x.Value}"));
    public string Bridge => Parameters["bridge"];
}