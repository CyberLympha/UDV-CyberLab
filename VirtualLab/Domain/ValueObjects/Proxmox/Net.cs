namespace VirtualLab.Domain.Value_Objects.Proxmox;

// todo: сделать лучше: потецинально, мы будет не только bridge делать поэтому Type есть.
public class Net
{
    private Dictionary<string, string> Parameters { get; set; } = new();
    public string Type { get; }
    
    
    
    // todo: пока что максимально простая реализация, потом допилить.
    public Net(string model, string bridge)
    {
        Parameters.Add("model", model);
        Parameters.Add("bridge", bridge);
        Type = "bridge";
    }

    public override string ToString()
    {
        return $"{Bridge}, {Parameters}";
    }

    public string this[string index] => Parameters[index];
    public string GetFull => string.Join(",", Parameters.Select(x => $"{x.Key}={x.Value}"));
    public string Bridge => Parameters["bridge"];
}