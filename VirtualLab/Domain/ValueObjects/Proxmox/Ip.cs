namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class Ip
{
    public string Value { get; set; }
    private int RangeNetwork { get; set; } = 24;

    public bool Empty => string.IsNullOrEmpty(Value);

    public string GetIdNetwork()
    {
        return string.Join(".", Value.Split(".")[..2]);
    }
}