namespace VirtualLab.Domain.Entities;

public class VirtualMachine
{
    public Guid Id { get; set; }
    public int ProxmoxId { get; set; }
    public Guid UserLabId { get; set; }
    public string Node { get; set; }

    public static VirtualMachine From(string node, int proxmoxVmId, Guid userLabId)
        => new()
        {
            Id = Guid.NewGuid(),
            ProxmoxId = proxmoxVmId,
            Node = node,
            UserLabId = userLabId
        };
}