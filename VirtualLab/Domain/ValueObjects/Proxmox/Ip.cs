namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class Ip
{
    public string Value { get; set; }
    private int RangeNetwork { get; set; } = 24;

    public string GetIdNetwork()
        => string.Join(".", Value.Split(".")[..2]);
}