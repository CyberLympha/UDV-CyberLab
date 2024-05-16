namespace VirtualLab.Domain.Entities;

public class VirtualMachine
{
    public Guid Id { get; set; }
    public int ProxmoxVmId { get; set; }
    public Guid UserLabId { get; set; }
    public string Node { get; set; }

    public static VirtualMachine From(string node, int proxmoxVmId, Guid userLabId)
        => new()
        {
            Id = Guid.NewGuid(),
            ProxmoxVmId = proxmoxVmId,
            Node = node,
            UserLabId = userLabId
        };
}